
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
        public List<Shelter> Find(ShelterStatus? status, bool? isActive)
        {
            var query = _context.Shelter
                .Include(s => s.ShelterFeatures)
                    .ThenInclude(sf => sf.Preference)
                .AsNoTracking();//לא לעקוב אחרי השינויים שנעשים בישויות  שנטענו מהמסד

            if (status.HasValue)
                query = query.Where(s => s.Status == status.Value);

            if (isActive.HasValue)
                query = query.Where(s => s.IsActive == isActive.Value);

            return query.ToList();
        }

        public List<Shelter> GetByUserId(int userId)
        {
            return _context.Shelter
                .Include(s => s.ShelterFeatures)
                    .ThenInclude(sf => sf.Preference)
                .AsNoTracking()
                .Where(s => s.CreatedByUserId == userId)
                .ToList();
        }

        public Shelter? GetById(int id)
        {
            return _context.Shelter
                          .Include(s => s.ShelterFeatures)
                              .ThenInclude(sf => sf.Preference)
                          .FirstOrDefault(s => s.ShelterId == id);
        }

        public Shelter Add(Shelter entity)
        {
            _context.Shelter.Add(entity);
            _context.SaveChanges();

            return _context.Shelter
                .Include(s => s.ShelterFeatures)
                    .ThenInclude(sf => sf.Preference)
                .First(s => s.ShelterId == entity.ShelterId);
        }

        public Shelter? Update(Shelter entity)
        {
            _context.Shelter.Update(entity);

            _context.SaveChanges();

            return _context.Shelter
                .Include(s => s.ShelterFeatures)
                    .ThenInclude(sf => sf.Preference)
                .First(s => s.ShelterId == entity.ShelterId);
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
        public Shelter? GetOwned(int id, int userId)
        {
            return _context.Shelter
                          .Include(s => s.ShelterFeatures)
                              .ThenInclude(sf => sf.Preference)
                .FirstOrDefault(s => s.ShelterId == id && s.CreatedByUserId == userId);
        }
    }
}
