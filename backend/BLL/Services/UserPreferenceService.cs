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
        private readonly IMapper _mapper;

        public UserPreferenceService(IUserPreferenceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<UserPreferenceDTO> GetAll()
        {
            var entities = _repository.GetAll();
            return _mapper.Map<List<UserPreferenceDTO>>(entities);
        }

        public UserPreferenceDTO? GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : _mapper.Map<UserPreferenceDTO>(entity);
        }

        public List<UserPreferenceDTO> GetByUserId(int userId)
        {
            var entities = _repository.GetByUserId(userId);
            return _mapper.Map<List<UserPreferenceDTO>>(entities);
        }

        public void SaveUserPreferences(int userId, SaveUserPreferencesRequestDTO dto)
        {
            // אם אותה העדפה נשלחה כמה פעמים, האחרונה קובעת (מונע קריסה על מפתח כפול)
            var entities = dto.Preferences
                .GroupBy(p => p.PreferenceId)
                .Select(g => g.Last())
                .Select(p => new DAL.UserPreference
                {
                    UserId = userId,
                    PreferenceId = p.PreferenceId,
                    Priority = p.Priority,
                    IsRequired = p.IsRequired
                })
                .ToList();

            _repository.ReplaceForUser(userId, entities);
        }
    }
}
