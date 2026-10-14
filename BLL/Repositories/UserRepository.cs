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
    public class UserRepository: IUserRepository
    {
        private readonly DB_Shelters _context;
        private readonly IMapper _mapper;

        public UserRepository(
            DB_Shelters context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<UserProfileDTO> GetAll()
        {
            return _context.User
                .AsNoTracking()
                .ProjectTo<UserProfileDTO>(
                    _mapper.ConfigurationProvider)
                .ToList();
        }

        public UserProfileDTO? GetById(int id)
        {
            return _context.User
                .Where(p => p.UserId == id)
                .ProjectTo<UserProfileDTO>(
                    _mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        //צריך לעשות פונקציות של הרשמה וכו
        //public PreferenceDTO Add(PreferenceDTO dto)
        //{
        //    var preference = _mapper.Map<Preference>(dto);

        //    _context.Preference.Add(preference);
        //    _context.SaveChanges();

        //    return _mapper.Map<PreferenceDTO>(preference);
        //}

        public UserProfileDTO? Update(int id, UpdateUserRequestDTO dto)
        {
            // שליפת המשתמש יחד עם ההעדפות הקיימות שלו מתוך ה-DAL
            var userProfile = _context.User
                .Include(u => u.UserPreferences) // ודא שאתה כולל את טבלת הקישור/ההעדפות
                .FirstOrDefault(p => p.UserId == id);

            if (userProfile == null)
                return null;

            // עדכון פרטי הבסיס של המשתמש
            userProfile.FullName = dto.FullName;
            userProfile.Email = dto.Email;

            // טיפול בעדכון ההעדפות (מחיקת ישנות והוספת החדשות, או עדכון לפי הלגיקה של הפרויקט שלך)
            // דוגמה בסיסית لإכלוס מחדש של ההעדפות בהתאם ל-DTO:
            _context.UserPreference.RemoveRange(userProfile.UserPreferences);

            foreach (var prefDto in dto.Preferences)
            {
                userProfile.UserPreferences.Add(new UserPreference
                {
                    UserId = id,
                    PreferenceId = prefDto.PreferenceId // בהנחה שיש מזהה להעדפה ב-DTO
                                                        // הוסף שדות נוספים במידת הצורך בהתאם למודל ה-DAL שלך
                });
            }

            _context.SaveChanges();

            // החזרת הפרופיל המעודכן ממופה ל-UserProfileDTO
            return _mapper.Map<UserProfileDTO>(userProfile);
        }

        public bool Delete(int id)
        {
            var user = _context.User
                .FirstOrDefault(p => p.UserId == id);

            if (user == null)
                return false;

            _context.User.Remove(user);
            _context.SaveChanges();

            return true;
        }
    }
}
