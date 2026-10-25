using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var entities = _repository.GetAll();
            return _mapper.Map<List<PreferenceDTO>>(entities);
        }

        public PreferenceDTO? GetById(int id)
        {
            var preference = _repository.GetById(id);
            return preference == null ? null : _mapper.Map<PreferenceDTO>(entity);
        }

        public PreferenceDTO Add(PreferenceDTO dto)
        {
            var name = dto.Name.Trim();

            // לא מוסיפים שוב העדפה שכבר קיימת בשם הזה
            if (_repository.GetAll().Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("כבר קיימת העדפה בשם הזה");

            var preference = _mapper.Map<DAL.Preference>(dto);
            preference.PreferenceId = 0;                    // המפתח נקבע ע"י ה-DB
            preference.Name = name;
            preference.Description = dto.Description ?? ""; // העמודות ב-DB לא מקבלות null
            preference.Category = dto.Category ?? "";

            return _mapper.Map<PreferenceDTO>(_repository.Add(preference));
        }

        public PreferenceDTO? Update(int id, PreferenceDTO dto)
        {
            var name = dto.Name.Trim();

            // שם כפול אצל העדפה אחרת - אותה העדפה יכולה לשמור על השם שלה
            if (_repository.GetAll().Any(p => p.PreferenceId != id &&
                                              string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("כבר קיימת העדפה אחרת בשם הזה");

            var entity = _mapper.Map<DAL.Preference>(dto);
            entity.Name = name;
            entity.Description = dto.Description ?? "";
            entity.Category = dto.Category ?? "";

            // ה-רפוסיטורי מעתיק רק שם, תיאור וקטגוריה. null = ההעדפה לא קיימת
            var updated = _repository.Update(id, entity);
            return updated == null ? null : _mapper.Map<PreferenceDTO>(updated);
        }

        public bool Delete(int id)
        {
            if (_repository.GetById(id) == null)
                return false;   // לא קיימת: ה-Controller מחזיר NotFound

            if (_repository.IsInUse(id))
                throw new InvalidOperationException("אי אפשר למחוק העדפה שנמצאת בשימוש אצל משתמשים או במקלטים");

            return _repository.Delete(id);
        }
    }
}

