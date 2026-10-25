
using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserPreferenceRepository: IUserPreferenceRepository
    {
        private readonly DB_Shelters _context;

        public UserPreferenceRepository(
            DB_Shelters context)        
        {
            _context = context;
        }

        public List<UserPreference> GetAll()
        {
            return _context.UserPreference
                            .AsNoTracking()
                            .Include(up => up.Preference)
                            .ToList();
        }

        //אם הפונקציה אמורה להחזיר העדפה ספציפית לפי מזהה ההעדפה עצמו
        public UserPreference? GetById(int id)
        {
            var userPreference = _context.UserPreference
                .Include(up => up.Preference)
                .FirstOrDefault(up => up.UserPreferenceId == id); // או שדה המפתח הראשי בטבלת הקישור

            if (userPreference == null)
                return null;

            return userPreference;
        }

        //הפונקציה מחזירה רשימת העדפות לפי קוד משתמש
        public List<UserPreference> GetByUserId(int userId) // או השאר GetById אבל עם List
        {
            return _context.UserPreference
                            .Where(up => up.UserId == userId)
                            .Include(up => up.Preference)
                            .ToList();
        }

        public UserPreference Add(UserPreference entity)
        {
            _context.UserPreference.Add(entity);
            _context.SaveChanges();
            return GetById(entity.UserPreferenceId)!;
        }

        public UserPreference? Update(UserPreference entity)
        {
            _context.UserPreference.Update(entity);
            _context.SaveChanges();

            // טעינה מחדש של האובייקט כולל הקשרים
            return GetById(entity.UserPreferenceId)!;
        }

        public bool Delete(int id)
        {
            var userPreference = _context.UserPreference
                .FirstOrDefault(up => up.UserPreferenceId == id);
            if (userPreference == null)
                return false;
            _context.UserPreference.Remove(userPreference);
            _context.SaveChanges();
            return true;
        }

    }
}

