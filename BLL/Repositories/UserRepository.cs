using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    internal class UserRepository
    {
        private readonly DB_Shelters _context;

        public UserRepository(DB_Shelters context)
        {
            _context = context;
        }

        public User GetById(int id)
        {
            // שימוש ב-Find לשлиפה מהירה לפי המפתח הראשי
            return _context.User.Find(id);
        }
        // 2. החזרת רשימת אובייקטים
        public IEnumerable<User> GetAll()
        {
            return _context.User.ToList();
        }

        // 3. הוספת אובייקט
        public void Add(User entity)
        {
            _context.User.Add(entity);
            _context.SaveChanges();
        }

        // 4. מחיקת אובייקט
        public void Delete(int id)
        {
            var entity = _context.User.Find(id);
            if (entity != null)
            {
                _context.User.Remove(entity);
                _context.SaveChanges();
            }
        }

        // 5. עדכון אובייקט
        public void Update(User entity)
        {
            _context.User.Update(entity);
            _context.SaveChanges();
        }
    }
}
