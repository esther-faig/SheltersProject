
using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ShelterFeatureRepository: IShelterFeatureRepository
    {
        private readonly DB_Shelters _context;

        public ShelterFeatureRepository(
            DB_Shelters context)
        {
            _context = context;
        }


        public List<ShelterFeature> GetAll()
        {
            return _context.ShelterFeature
                .AsNoTracking()
                .Include(sf => sf.Preference)
                .ToList();
        }

        public ShelterFeature? GetById(int id)
        {
            return _context.ShelterFeature
                .Include(sf => sf.Preference)
                .FirstOrDefault(sf => sf.ShelterFeatureId == id);
        }

        public List<ShelterFeature> GetByShelterId(int shelterId)
        {
            return _context.ShelterFeature
                .Include(sf => sf.Preference)
                .Where(sf => sf.ShelterId == shelterId)
                .ToList();
        }
        public ShelterFeature Add(ShelterFeature entity)
        {
            _context.ShelterFeature.Add(entity);
            _context.SaveChanges();
            return GetById(entity.ShelterFeatureId)!;
        }

        public ShelterFeature? Update(ShelterFeature entity)
        {
            _context.ShelterFeature.Update(entity);

            _context.SaveChanges();
            return GetById(entity.ShelterFeatureId)!;
        }

        public bool Delete(int id)
        {
            var shelterFeature = _context.ShelterFeature
                .FirstOrDefault(p => p.ShelterFeatureId == id);

            if (shelterFeature == null)
                return false;

            _context.ShelterFeature.Remove(shelterFeature);
            _context.SaveChanges();

            return true;
        }
    }
}
