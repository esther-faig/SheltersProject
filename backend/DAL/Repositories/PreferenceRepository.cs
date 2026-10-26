using DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly DB_Shelters _context;
        public PreferenceRepository(
            DB_Shelters context)
        {
            _context = context;
        }

        public List<Preference> GetAll()
        {
            return _context.Preference
                 .AsNoTracking()
                 .ToList();
        }


        public Preference? GetById(int id)
        {
            return _context.Preference
                .FirstOrDefault(p => p.PreferenceId == id);
        }

        public Preference Add(Preference entity)
        {
            _context.Preference.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Preference? Update(int id, Preference entity)
        {
            // טוענים את ההעדפה הקיימת (עם מעקב) לפי ה-id מה-URL
            var existing = _context.Preference
                .FirstOrDefault(p => p.PreferenceId == id);

            if (existing == null)
                return null;   // ה-Service מחזיר null, והקונטרולר מחזיר NotFound

            // מעתיקים רק את מה שמותר לשנות
            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.Category = entity.Category;

            _context.SaveChanges();
            return existing;
        }

        public bool Delete(int id)
        {
            var preference = _context.Preference
                .FirstOrDefault(p => p.PreferenceId == id);

            if (preference == null)
                return false;

            _context.Preference.Remove(preference);
            _context.SaveChanges();

            return true;
        }
        public bool IsInUse(int id)
        {
            return _context.UserPreference.Any(up => up.PreferenceId == id)
                || _context.ShelterFeature.Any(sf => sf.PreferenceId == id);
        }
    }
}