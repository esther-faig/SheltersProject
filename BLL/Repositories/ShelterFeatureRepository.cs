using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    internal class ShelterFeatureRepository
    {
        private readonly DB_Shelters _context; 

        public ShelterFeatureRepository(DB_Shelters context)
        {
            _context = context;
        }

        public ShelterFeature GetById(int id)
        {
            // שימוש ב-Find לשлиפה מהירה לפי המפתח הראשי
            return _context.ShelterFeature.Find(id);
        }

        // 2. החזרת רשימת אובייקטים
        public IEnumerable<ShelterFeature> GetAll()
        {
            return _context.ShelterFeature.ToList();
        }

        // 3. הוספת אובייקט
        public void Add(ShelterFeature entity)
        {
            _context.ShelterFeature.Add(entity);
            _context.SaveChanges();
        }

        // 4. מחיקת אובייקט
        public void Delete(int id)
        {
            var entity = _context.ShelterFeature.Find(id);
            if (entity != null)
            {
                _context.ShelterFeature.Remove(entity);
                _context.SaveChanges();
            }
        }

        // 5. עדכון אובייקט
        public void Update(ShelterFeature entity)
        {
            _context.ShelterFeature.Update(entity);
            _context.SaveChanges();
        }
    }
}
