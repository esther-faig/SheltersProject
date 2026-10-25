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

            //בדיקה שההעדפות קימות במערכת
            var validIds = _preferenceRepository.GetAll().Select(p => p.PreferenceId).ToHashSet();
            if (incoming.Any(p => !validIds.Contains(p.PreferenceId)))
                throw new InvalidOperationException("נשלחה העדפה שלא קיימת");
            //TOHASHSET- ממירה אוסף נתונים לקבוצה של ערכים יחודים שמייעלת חיפושים

            var existing = _repository.GetByUserId(userId);
            var incomingIds = incoming.Select(p => p.PreferenceId).ToHashSet();


            var toRemove = existing.Where(e => !incomingIds.Contains(e.PreferenceId)).ToList();
            var toAdd = new List<DAL.UserPreference>();

            foreach (var p in incoming)
            {
                var current = existing.FirstOrDefault(e => e.PreferenceId == p.PreferenceId);

                if (current != null)
                {
                    //קיימת--
                    // משנים בזיכרון, והשמירה קורית ב-SaveBatch
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
