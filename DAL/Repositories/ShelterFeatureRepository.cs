
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
                .ToList();
        }

        public ShelterFeature? GetById(int id)
        {
            return _context.ShelterFeature
                .FirstOrDefault(p => p.ShelterFeatureId == id);
        }

        public ShelterFeature Add(ShelterFeature entity)
        {
            _context.ShelterFeature.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public ShelterFeature? Update(int id, ShelterFeature entity)
        {
            var shelterFeature = _context.ShelterFeature
                .FirstOrDefault(sf => sf.ShelterFeatureId == id);

            if (shelterFeature == null)
                return null;

            _context.Entry(shelterFeature).CurrentValues.SetValues(entity);

            shelterFeature.ShelterFeatureId = id;
            _context.SaveChanges();
            return shelterFeature;
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
