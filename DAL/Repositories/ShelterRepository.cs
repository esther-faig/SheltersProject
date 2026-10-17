
using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ShelterRepository: IShelterRepository
    {
        private readonly DB_Shelters _context;

        public ShelterRepository(
            DB_Shelters context)
        {
            _context = context;
        }

        public List<Shelter> GetAll()
        {
            return _context.Shelter
                            .AsNoTracking()
                            .ToList();
        }

        public Shelter? GetById(int id)
        {
            return _context.Shelter
                            .FirstOrDefault(p => p.ShelterId == id);
        }

public Shelter Add(Shelter entity)
        {
            _context.Shelter.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Shelter? Update(int id, Shelter entity)
        {
            var shelter = _context.Shelter
            .Include(s => s.ShelterFeatures) // כולל את הטבלאות המקושרות אם צריך לעדכן אותן
            .FirstOrDefault(sf => sf.ShelterId == id);

            if (shelter == null)
                return null;

            // 1. מעתיק אוטומטית את כל ערכי השדות הפשוטים 
            _context.Entry(shelter).CurrentValues.SetValues(entity);

            shelter.ShelterId = id;
            // 2. טיפול בעדכון רשימת המאפיינים 
            if (entity.ShelterFeatures != null)
            {
                //מחיקת הקשרים הישנים והוספת החדשים
                _context.ShelterFeature.RemoveRange(shelter.ShelterFeatures);
                shelter.ShelterFeatures = entity.ShelterFeatures;
            }

            _context.SaveChanges();

            return shelter;
        }
        public bool Delete(int id)
        {
            var shelter = _context.Shelter
                .FirstOrDefault(p => p.ShelterId == id);

            if (shelter == null)
                return false;

            _context.Shelter.Remove(shelter);
            _context.SaveChanges();

            return true;
        }
    }
}
