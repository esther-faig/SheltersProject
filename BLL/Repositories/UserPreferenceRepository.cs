using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class UserPreferenceRepository: IUserPreferenceRepository
    {
        private readonly DB_Shelters _context;
        private readonly IMapper _mapper;

        public UserPreferenceRepository(
            DB_Shelters context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<UserPreferenceDTO> GetAll()
        {
            return _context.UserPreference
                .AsNoTracking()
                .ProjectTo<UserPreferenceDTO>(
                    _mapper.ConfigurationProvider)
                .ToList();
        }

        //אם הפונקציה אמורה להחזיר העדפה ספציפית לפי מזהה ההעדפה עצמו
        public UserPreferenceDTO? GetById(int id)
        {
            var userPreference = _context.UserPreference
                .Include(up => up.Preference)
                .FirstOrDefault(up => up.UserPreferenceId == id); // או שדה המפתח הראשי בטבלת הקישור

            if (userPreference == null)
                return null;

            return _mapper.Map<UserPreferenceDTO>(userPreference);
        }

        //הפונקציה מחזירה רשימת העדפות לפי קוד משתמש
        public List<UserPreferenceDTO> GetByUserId(int userId) // או השאר GetById אבל עם List
        {
            var userPreferences = _context.UserPreference
                .Where(up => up.UserId == userId)
                .Include(up => up.Preference)
                .ToList();

            return _mapper.Map<List<UserPreferenceDTO>>(userPreferences);
        }

        public void SaveUserPreferences(int userId, SaveUserPreferencesRequestDTO dto)
        {
            // הסרת ההעדפות הקיימות של המשתמש כדי לעדכן אותן בחדשות (או לבצע לוגיקת Upsert בהתאם לצורך)
            var existingPreferences = _context.UserPreference
                .Where(up => up.UserId == userId)
                .ToList();

            if (existingPreferences.Any())
            {
                _context.UserPreference.RemoveRange(existingPreferences);
            }

            // הוספת ההעדפות החדשות שהתקבלו ב-DTO
            foreach (var pref in dto.Preferences) // בהנחה שה-DTO מכיל רשימת מזהים, או רשימת אובייקטים בהתאם למבנה אצלך
            {
                var userPreference = new UserPreference
                {
                    UserId = userId,
                    PreferenceId = pref.PreferenceId // או השדה המתאים ב-DTO
                };
                _context.UserPreference.Add(userPreference);
            }

            _context.SaveChanges();
        }
    }


}

