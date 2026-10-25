using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using DAL.Repositories;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ShelterService : IShelterService
    {
        private readonly IShelterRepository _repository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IAlertZoneService _alertZoneService;
        private readonly IMapper _mapper;

        public ShelterService(
            IShelterRepository repository,
            IPreferenceRepository preferenceRepository,
            IAlertZoneService alertZoneService,
            IMapper mapper)
        {
            _repository = repository;
            _preferenceRepository = preferenceRepository;
            _alertZoneService = alertZoneService;
            _mapper = mapper;
        }

        public List<ShelterSearchResultDTO> GetAll()
        {
            var shelters = _repository.Find(ShelterStatus.Approved, true);
            return _mapper.Map<List<ShelterSearchResultDTO>>(shelters);
        }

        public ShelterSearchResultDTO? GetById(int id)
        {
            var shelter = _repository.GetById(id);
            if (shelter == null || shelter.Status != ShelterStatus.Approved || !shelter.IsActive)
                return null;

            return _mapper.Map<ShelterSearchResultDTO>(shelter);
        }

        public List<SharedShelterDTO> GetMyShelters(int userId)
        {
            return _mapper.Map<List<SharedShelterDTO>>(_repository.GetByUserId(userId));
        }

        public ShelterDetailsDTO? GetMyShelter(int id, int userId)
        {
            var shelter = _repository.GetOwned(id, userId);
            return shelter == null ? null : _mapper.Map<ShelterDetailsDTO>(shelter);
        }

        public ShelterDetailsDTO Add(int userId, ShelterRequestDTO dto)
        {
            // אם אותה העדפה נשלחה כמה פעמים, האחרונה קובעת
            var features = dto.Features
                .GroupBy(f => f.PreferenceId)
                .Select(g => g.Last())
                .ToList();

            var validIds = _preferenceRepository.GetAll().Select(p => p.PreferenceId).ToHashSet();
            if (features.Any(f => !validIds.Contains(f.PreferenceId)))
                throw new InvalidOperationException("נשלחה העדפה שלא קיימת");

            var shelter = _mapper.Map<DAL.Shelter>(dto);
            shelter.ShelterFeatures = _mapper.Map<List<ShelterFeature>>(features);
            shelter.CreatedByUserId = userId;
            shelter.Status = ShelterStatus.Pending;
            shelter.IsActive = true;
            shelter.AlertZoneId = _alertZoneService.FindZoneIdByLocation(dto.Latitude, dto.Longitude);

            return _mapper.Map<ShelterDetailsDTO>(_repository.Add(shelter));
        }

        public ShelterDetailsDTO? Update(int id, int userId, ShelterRequestDTO dto)
        {
            var shelter = _repository.GetOwned(id, userId);
            if (shelter == null)
                return null;   // לא קיים או לא שלו

            var features = dto.Features
                .GroupBy(f => f.PreferenceId)
                .Select(g => g.Last())
                .ToList();

            var validIds = _preferenceRepository.GetAll().Select(p => p.PreferenceId).ToHashSet();
            if (features.Any(f => !validIds.Contains(f.PreferenceId)))
                throw new InvalidOperationException("נשלחה העדפה שלא קיימת");

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
            shelter.Files = new List<string>(dto.Files);

            // מאפיינים: מסירים מה שנעלם, מעדכנים קיים, מוסיפים חדש
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

            return _mapper.Map<ShelterDetailsDTO>(_repository.Update(shelter));
        }

        public ShelterDetailsDTO? UpdateAvailability(int id, int userId, UpdateShelterAvailabilityDTO dto)
        {
            var shelter = _repository.GetOwned(id, userId);
            if (shelter == null)
                return null;

            shelter.IsActive = dto.IsActive;

            return _mapper.Map<ShelterDetailsDTO>(_repository.Update(shelter));
        }

        public bool Delete(int id, int userId)
        {
            // מחפשים מקלט עם המספר הזה *וגם* של המשתמש המחובר
            var shelter = _repository.GetOwned(id, userId);
            if (shelter == null)
                return false;

            _repository.Delete(id);
            return true;
        }

        public List<AdminShelterDTO> GetByStatus(string? status)
        {
            // הערך כבר אומת ב-קונטרולר (RegularExpression), לכן ההמרה בטוחה
            ShelterStatus? parsed = string.IsNullOrWhiteSpace(status)
                ? null
                : Enum.Parse<ShelterStatus>(status);

            return _mapper.Map<List<AdminShelterDTO>>(_repository.Find(parsed, null));
        }

        public ShelterDetailsDTO? UpdateStatus(int id, UpdateShelterStatusDTO dto)
        {
            // טוענים את המקלט (עם מעקב) כדי שאפשר יהיה לשנות אותו ולשמור
            var shelter = _repository.GetById(id);
            if (shelter == null)
                return null;   // ה-Controller מחזיר NotFound

            // AutoMapper ממיר את הטקסט לסטטוס ומעתיק גם את סיבת הדחייה על המקלט הקיים
            _mapper.Map(dto, shelter);

            // כלל עסקי: דחייה חייבת סיבה
            if (shelter.Status == ShelterStatus.Rejected && string.IsNullOrWhiteSpace(shelter.RejectionReason))
                throw new InvalidOperationException("בדחיית מקלט חובה לציין סיבה");

            // סיבת דחייה נשמרת רק למקלט שנדחה (אישור או חזרה להמתנה מנקים אותה)
            if (shelter.Status != ShelterStatus.Rejected)
                shelter.RejectionReason = null;

            // שומרים, טוענים מחדש עם Include וממפים ל-DTO
            return _mapper.Map<ShelterDetailsDTO>(_repository.Update(shelter));
        }
    }
}
