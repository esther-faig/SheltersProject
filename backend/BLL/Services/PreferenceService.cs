using AutoMapper;
using BLL.Exceptions;
using DAL.Repositories;
using DTO;

namespace BLL.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IPreferenceRepository _repository;
        private readonly IMapper _mapper;

        public PreferenceService(IPreferenceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<PreferenceDTO> GetAll()
        {
            return _mapper.Map<List<PreferenceDTO>>(_repository.GetAll());
        }

        public PreferenceDTO GetById(int id)
        {
            var preference = _repository.GetById(id)
                ?? throw new NotFoundException($"העדפה {id} לא נמצאה");
            return _mapper.Map<PreferenceDTO>(preference);
        }

        public PreferenceDTO Add(PreferenceDTO dto)
        {
            var name = dto.Name.Trim();

            // לא מוסיפים שוב העדפה שכבר קיימת בשם הזה
            if (_repository.GetAll().Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
                throw new ConflictException("כבר קיימת העדפה בשם הזה");

            var preference = _mapper.Map<DAL.Preference>(dto);
            preference.PreferenceId = 0;                    // המפתח נקבע ע"י ה-DB
            preference.Name = name;
            preference.Description = dto.Description ?? ""; // העמודות ב-DB לא מקבלות null
            preference.Category = dto.Category ?? "";

            return _mapper.Map<PreferenceDTO>(_repository.Add(preference));
        }

        public PreferenceDTO Update(int id, PreferenceDTO dto)
        {
            // טוענים את ההעדפה הקיימת (עם מעקב) ומעדכנים אותה, במקום ליצור ישות חדשה עם מזהה 0
            var existing = _repository.GetById(id)
                ?? throw new NotFoundException($"העדפה {id} לא נמצאה");

            var name = dto.Name.Trim();

            // שם כפול אצל העדפה אחרת - אותה העדפה יכולה לשמור על השם שלה
            if (_repository.GetAll().Any(p => p.PreferenceId != id &&
                                              string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
                throw new ConflictException("כבר קיימת העדפה אחרת בשם הזה");

            existing.Name = name;
            existing.Description = dto.Description ?? "";
            existing.Category = dto.Category ?? "";

            return _mapper.Map<PreferenceDTO>(_repository.Update(id, existing));
        }

        public void Delete(int id)
        {
            if (_repository.GetById(id) == null)
                throw new NotFoundException($"העדפה {id} לא נמצאה");

            if (_repository.IsInUse(id))
                throw new ConflictException("אי אפשר למחוק העדפה שנמצאת בשימוש אצל משתמשים או במקלטים");

            _repository.Delete(id);
        }
    }
}