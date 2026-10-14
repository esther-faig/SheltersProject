using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    internal class UserPreferenceRepository
    {
        private readonly DB_Shelters _context;

        public UserPreferenceRepository(DB_Shelters context)
        {
            _context = context;
        }

        public UserPreference GetById(int id)
        {
            // שימוש ב-Find לשлиפה מהירה לפי המפתח הראשי
            return _context.UserPreference.Find(id);
        }

        // 2. החזרת רשימת אובייקטים
        public IEnumerable<UserPreference> GetAll()
        {
            return _context.UserPreference.ToList();
        }

        // 3. הוספת אובייקט
        public void Add(UserPreference entity)
        {
            _context.UserPreference.Add(entity);
            _context.SaveChanges();
        }

        // 4. מחיקת אובייקט
        public void Delete(int id)
        {
            var entity = _context.UserPreference.Find(id);
            if (entity != null)
            {
                _context.UserPreference.Remove(entity);
                _context.SaveChanges();
            }
        }

        // 5. עדכון אובייקט
        public void Update(UserPreference entity)
        {
            _context.UserPreference.Update(entity);
            _context.SaveChanges();
        }
    }
}
