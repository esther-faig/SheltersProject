using AutoMapper;
using BLL.Exceptions;
using DAL.Repositories;
using DTO;

namespace BLL.Services
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly IUserPreferenceRepository _repository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserPreferenceService(
            IUserPreferenceRepository repository,
            IPreferenceRepository preferenceRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _repository = repository;
            _preferenceRepository = preferenceRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<UserPreferenceDTO> GetAll()
        {
            return _mapper.Map<List<UserPreferenceDTO>>(_repository.GetAll());
        }

        public UserPreferenceDTO GetById(int id)
        {
            var userPreference = _repository.GetById(id)
                ?? throw new NotFoundException($"העדפת משתמש {id} לא נמצאה");
            return _mapper.Map<UserPreferenceDTO>(userPreference);
        }

        public List<UserPreferenceDTO> GetByUserId(int userId)
        {
            return _mapper.Map<List<UserPreferenceDTO>>(_repository.GetByUserId(userId));
        }

        public void SaveUserPreferences(int userId, SaveUserPreferencesRequestDTO dto)
        {
            // בלי זה, משתמש לא קיים גורם לשגיאת מפתח זר במסד (500)
            if (!_userRepository.Exists(userId))
                throw new NotFoundException($"משתמש {userId} לא נמצא");

            // אם אותה העדפה נשלחה כמה פעמים, האחרונה קובעת
            var incoming = dto.Preferences
                .GroupBy(p => p.PreferenceId)
                .Select(g => g.Last())
                .ToList();

            //בדיקה שההעדפות קימות במערכת
            var validIds = _preferenceRepository.GetAll().Select(p => p.PreferenceId).ToHashSet();
            if (incoming.Any(p => !validIds.Contains(p.PreferenceId)))
                throw new BusinessRuleException("נשלחה העדפה שלא קיימת");

            var existing = _repository.GetByUserId(userId);
            var incomingIds = incoming.Select(p => p.PreferenceId).ToHashSet();

            var toRemove = existing.Where(e => !incomingIds.Contains(e.PreferenceId)).ToList();
            var toAdd = new List<DAL.UserPreference>();

            foreach (var p in incoming)
            {
                var current = existing.FirstOrDefault(e => e.PreferenceId == p.PreferenceId);

                if (current != null)
                {
                    // קיימת: משנים בזיכרון, והשמירה קורית ב-SaveBatch
                    current.Priority = p.Priority;
                    current.IsRequired = p.IsRequired;
                }
                else
                {
                    // חדשה: מוסיפים
                    toAdd.Add(new DAL.UserPreference
                    {
                        UserId = userId,
                        PreferenceId = p.PreferenceId,
                        Priority = p.Priority,
                        IsRequired = p.IsRequired
                    });
                }
            }

            _repository.SaveBatch(toAdd, toRemove);
        }
    }
}