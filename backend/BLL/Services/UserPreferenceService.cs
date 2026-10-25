using AutoMapper;
using DAL.Repositories;
using DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly IUserPreferenceRepository _repository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IMapper _mapper;

        public UserPreferenceService(
            IUserPreferenceRepository repository,
            IPreferenceRepository preferenceRepository,
            IMapper mapper)
        {
            _repository = repository;
            _preferenceRepository = preferenceRepository;
            _mapper = mapper;
        }
        public List<UserPreferenceDTO> GetAll()
        {
            return _mapper.Map<List<UserPreferenceDTO>>(_repository.GetAll());
        }

        public UserPreferenceDTO? GetById(int id)
        {
            var userPreference = _repository.GetById(id);
            return userPreference == null ? null : _mapper.Map<UserPreferenceDTO>(userPreference);
        }
        public List<UserPreferenceDTO> GetByUserId(int userId)
        {
            return _mapper.Map<List<UserPreferenceDTO>>(_repository.GetByUserId(userId));
        }

        public void SaveUserPreferences(int userId, SaveUserPreferencesRequestDTO dto)
        {
            // אם אותה העדפה נשלחה כמה פעמים, האחרונה קובעת
            var incoming = dto.Preferences
                .GroupBy(p => p.PreferenceId)
                .Select(g => g.Last())
                .ToList();

            var validIds = _preferenceRepository.GetAll().Select(p => p.PreferenceId).ToHashSet();
            if (incoming.Any(p => !validIds.Contains(p.PreferenceId)))
                throw new InvalidOperationException("נשלחה העדפה שלא קיימת");

            var existing = _repository.GetByUserId(userId);
            var incomingIds = incoming.Select(p => p.PreferenceId).ToHashSet();

            // מוחקים מה שהמשתמש הסיר
            foreach (var old in existing.Where(e => !incomingIds.Contains(e.PreferenceId)).ToList())
                _repository.Delete(old.UserPreferenceId);

            foreach (var p in incoming)
            {
                var current = existing.FirstOrDefault(e => e.PreferenceId == p.PreferenceId);

                if (current != null)
                {
                    // קיימת: מעדכנים עדיפות וחובה
                    current.Priority = p.Priority;
                    current.IsRequired = p.IsRequired;
                    _repository.Update(current);
                }
                else
                {
                    // חדשה: מוסיפים
                    _repository.Add(new DAL.UserPreference
                    {
                        UserId = userId,
                        PreferenceId = p.PreferenceId,
                        Priority = p.Priority,
                        IsRequired = p.IsRequired
                    });
                }

            }
}
