using AutoMapper;
using BLL.Exceptions;
using DAL;
using DAL.Repositories;
using DTO;

namespace BLL.Services
{
    public class ShelterService : IShelterService
    {
        private readonly IShelterRepository _repository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAlertZoneService _alertZoneService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private const int MaxImages = 5;

        public ShelterService(
            IShelterRepository repository,
            IPreferenceRepository preferenceRepository,
            IUserRepository userRepository,
            IAlertZoneService alertZoneService,
            IMapper mapper,
            IImageService imageService)
        {
            _repository = repository;
            _preferenceRepository = preferenceRepository;
            _userRepository = userRepository;
            _alertZoneService = alertZoneService;
            _mapper = mapper;
            _imageService = imageService;
        }

        public List<ShelterSearchResultDTO> GetAll()
        {
            var shelters = _repository.Find(ShelterStatus.Approved, true);
            return _mapper.Map<List<ShelterSearchResultDTO>>(shelters);
        }

        public ShelterSearchResultDTO GetById(int id)
        {
            var shelter = _repository.GetById(id);
            if (shelter == null || shelter.Status != ShelterStatus.Approved || !shelter.IsActive)
                throw new NotFoundException($"מקלט {id} לא נמצא");

            return _mapper.Map<ShelterSearchResultDTO>(shelter);
        }

        public List<SharedShelterDTO> GetMyShelters(int userId)
        {
            return _mapper.Map<List<SharedShelterDTO>>(_repository.GetByUserId(userId));
        }

        public ShelterDetailsDTO GetMyShelter(int id, int userId)
        {
            // לא קיים או לא שלך: אותה תשובה
            var shelter = _repository.GetOwned(id, userId)
                ?? throw new NotFoundException($"מקלט {id} לא נמצא");
            return _mapper.Map<ShelterDetailsDTO>(shelter);
        }

        // אם אותה העדפה נשלחה כמה פעמים האחרונה קובעת, וכל ההעדפות חייבות להיות קיימות
        private List<ShelterFeatureUpdateDTO> GetValidFeatures(ShelterRequestDTO dto)
        {
            var features = dto.Features
                .GroupBy(f => f.PreferenceId)
                .Select(g => g.Last())
                .ToList();

            var validIds = _preferenceRepository.GetAll().Select(p => p.PreferenceId).ToHashSet();
            if (features.Any(f => !validIds.Contains(f.PreferenceId)))
                throw new BusinessRuleException("נשלחה העדפה שלא קיימת");

            return features;
        }

        public ShelterDetailsDTO Add(int userId, ShelterRequestDTO dto)
        {
            // כל הבדיקות לפני שמירת קבצים, כדי לא לשמור תמונות בשביל מקלט שלא יישמר
            if (!_userRepository.Exists(userId))
                throw new NotFoundException($"משתמש {userId} לא נמצא");

            var features = GetValidFeatures(dto);

            if (dto.ImageFiles.Count > MaxImages)
                throw new BusinessRuleException($"אפשר להעלות עד {MaxImages} תמונות");

            var shelter = _mapper.Map<DAL.Shelter>(dto);
            shelter.ShelterFeatures = _mapper.Map<List<ShelterFeature>>(features);
            shelter.CreatedByUserId = userId;
            shelter.Status = ShelterStatus.Pending;
            shelter.IsActive = true;
            shelter.AlertZoneId = _alertZoneService.FindZoneIdByLocation(dto.Latitude, dto.Longitude);

            var saved = new List<string>();
            // try/catch מקומי: כאן עושים משהו עם השגיאה (מוחקים קבצים יתומים) וממשיכים לזרוק
            try
            {
                foreach (var file in dto.ImageFiles)
                    saved.Add(_imageService.Save(file));
                shelter.Files = saved;

                return _mapper.Map<ShelterDetailsDTO>(_repository.Add(shelter));
            }
            catch
            {
                foreach (var name in saved) _imageService.Delete(name);   // לא משאירים קבצים יתומים
                throw;
            }
        }

        public ShelterDetailsDTO Update(int id, int userId, ShelterRequestDTO dto)
        {
            var shelter = _repository.GetOwned(id, userId)
                ?? throw new NotFoundException($"מקלט {id} לא נמצא");   // לא קיים או לא שלך

            var features = GetValidFeatures(dto);

            var keep = dto.ExistingFiles.Where(f => shelter.Files.Contains(f)).Distinct().ToList();
            var removed = shelter.Files.Except(keep).ToList();
            if (keep.Count + dto.ImageFiles.Count > MaxImages)
                throw new BusinessRuleException($"אפשר להעלות עד {MaxImages} תמונות");

            // שינוי מיקום: מחשבים אזור מחדש, והמקלט חוזר לאישור
            if (shelter.Latitude != dto.Latitude || shelter.Longitude != dto.Longitude)
            {
                shelter.AlertZoneId = _alertZoneService.FindZoneIdByLocation(dto.Latitude, dto.Longitude);
                shelter.Status = ShelterStatus.Pending;
                shelter.RejectionReason = null;
            }

            // רק שדות שמותר לערוך. הסטטוס, הבעלים ו-IsActive לא נדרסים
            shelter.Name = dto.Name;
            shelter.ShelterType = dto.ShelterType;
            shelter.Address = dto.Address;
            shelter.City = dto.City;
            shelter.Latitude = dto.Latitude;
            shelter.Longitude = dto.Longitude;
            shelter.MaxCapacity = dto.MaxCapacity;

            // מאפיינים: מסירים מה שנעלם, מעדכנים קיים, מוסיפים חדש.
            // נעשה לפני השמירה, כדי שהכול יישמר באותה פעולה
            var incomingIds = features.Select(f => f.PreferenceId).ToHashSet();

            foreach (var old in shelter.ShelterFeatures.Where(f => !incomingIds.Contains(f.PreferenceId)).ToList())
                shelter.ShelterFeatures.Remove(old);

            foreach (var f in features)
            {
                var existing = shelter.ShelterFeatures.FirstOrDefault(x => x.PreferenceId == f.PreferenceId);
                if (existing != null)
                    existing.IsAvailable = f.IsAvailable;
                else
                    shelter.ShelterFeatures.Add(new ShelterFeature { PreferenceId = f.PreferenceId, IsAvailable = f.IsAvailable });
            }

            var newFiles = new List<string>();
            ShelterDetailsDTO result;
            try
            {
                foreach (var file in dto.ImageFiles)
                    newFiles.Add(_imageService.Save(file));

                shelter.Files = keep.Concat(newFiles).ToList();   // הישנים שנשמרו + החדשים

                result = _mapper.Map<ShelterDetailsDTO>(_repository.Update(shelter));
            }
            catch
            {
                foreach (var name in newFiles) _imageService.Delete(name);   // השמירה נכשלה: מוחקים את החדשים
                throw;
            }

            // מוחקים את הישנים רק אחרי ששמירה במסד הצליחה
            foreach (var name in removed) _imageService.Delete(name);

            return result;
        }

        public ShelterDetailsDTO UpdateAvailability(int id, int userId, UpdateShelterAvailabilityDTO dto)
        {
            var shelter = _repository.GetOwned(id, userId)
                ?? throw new NotFoundException($"מקלט {id} לא נמצא");

            shelter.IsActive = dto.IsActive;

            return _mapper.Map<ShelterDetailsDTO>(_repository.Update(shelter));
        }

        public void Delete(int id, int userId)
        {
            // מחפשים מקלט עם המספר הזה *וגם* של המשתמש המחובר
            var shelter = _repository.GetOwned(id, userId)
                ?? throw new NotFoundException($"מקלט {id} לא נמצא");

            var files = shelter.Files.ToList();
            _repository.Delete(id);
            foreach (var f in files) _imageService.Delete(f);
        }

        public List<AdminShelterDTO> GetByStatus(string? status)
        {
            // הערך כבר אומת ב-Controller (RegularExpression), לכן ההמרה בטוחה
            ShelterStatus? parsed = string.IsNullOrWhiteSpace(status)
                ? null
                : Enum.Parse<ShelterStatus>(status);

            return _mapper.Map<List<AdminShelterDTO>>(_repository.Find(parsed, null));
        }

        public ShelterDetailsDTO UpdateStatus(int id, UpdateShelterStatusDTO dto)
        {
            // טוענים את המקלט (עם מעקב) כדי שאפשר יהיה לשנות אותו ולשמור
            var shelter = _repository.GetById(id)
                ?? throw new NotFoundException($"מקלט {id} לא נמצא");

            // כלל עסקי (בודקים לפני המיפוי): דחייה חייבת סיבה
            if (dto.Status == nameof(ShelterStatus.Rejected) && string.IsNullOrWhiteSpace(dto.RejectionReason))
                throw new BusinessRuleException("בדחיית מקלט חובה לציין סיבה");

            // AutoMapper ממיר את הטקסט לסטטוס ומעתיק גם את סיבת הדחייה על המקלט הקיים
            _mapper.Map(dto, shelter);

            // סיבת דחייה נשמרת רק למקלט שנדחה (אישור או חזרה להמתנה מנקים אותה)
            if (shelter.Status != ShelterStatus.Rejected)
                shelter.RejectionReason = null;

            return _mapper.Map<ShelterDetailsDTO>(_repository.Update(shelter));
        }
    }
}