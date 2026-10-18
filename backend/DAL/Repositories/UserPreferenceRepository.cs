
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
            return entity;
        }

        public UserPreference? Update(int id, UserPreference entity)
        {
            var userPreference = _context.UserPreference
                .FirstOrDefault(up => up.UserPreferenceId == id);
            if (userPreference == null)
                return null;
            // עדכון השדות הרלוונטיים
            _context.Entry(userPreference).CurrentValues.SetValues(entity);
            userPreference.UserPreferenceId = id;
            _context.SaveChanges();
            return userPreference;
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
        public void ReplaceForUser(int userId, List<UserPreference> preferences)
        {
            var existing = _context.UserPreference
                .Where(up => up.UserId == userId)
                .ToList();

            var incomingIds = preferences.Select(p => p.PreferenceId).ToHashSet();

            // מוחקים את מה שהמשתמש הסיר
            _context.UserPreference.RemoveRange(
                existing.Where(e => !incomingIds.Contains(e.PreferenceId)));

            foreach (var incoming in preferences)
            {
                var current = existing.FirstOrDefault(e => e.PreferenceId == incoming.PreferenceId);
                if (current != null)
                {
                    // קיימת: מעדכנים עדיפות וחובה
                    current.Priority = incoming.Priority;
                    current.IsRequired = incoming.IsRequired;
                }
                else
                {
                    _context.UserPreference.Add(new UserPreference
                    {
                        UserId = userId,
                        PreferenceId = incoming.PreferenceId,
                        Priority = incoming.Priority,
                        IsRequired = incoming.IsRequired
                    });
                }
            }

            // SaveChanges אחד: או שהכול נשמר או שלא נשמר כלום
            _context.SaveChanges();
        }

    }


}

