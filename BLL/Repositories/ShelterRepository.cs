using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    internal class ShelterRepository
    {
        private readonly DB_Shelters _context;

        public ShelterRepository(DB_Shelters context)
        {
            _context = context;
        }

        public Shelter GetById(int id)
        {
            // שימוש ב-Find לשлиפה מהירה לפי המפתח הראשי
            return _context.Shelter.Find(id);
        }

        // 2. החזרת רשימת אובייקטים
        public IEnumerable<Shelter> GetAll()
        {
            return _context.Shelter.ToList();
        }

        // 3. הוספת אובייקט
        public void Add(Shelter entity)
        {
            _context.Shelter.Add(entity);
            _context.SaveChanges();
        }

        // 4. מחיקת אובייקט
        public void Delete(int id)
        {
            var entity = _context.Shelter.Find(id);
            if (entity != null)
            {
                _context.Shelter.Remove(entity);
                _context.SaveChanges();
            }
        }

        // 5. עדכון אובייקט
        public void Update(Shelter entity)
        {
            _context.Shelter.Update(entity);
            _context.SaveChanges();
        }
    }
}
