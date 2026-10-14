using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class ShelterFeatureRepository: IShelterFeatureRepository
    {
        private readonly DB_Shelters _context;
        private readonly IMapper _mapper;

        public ShelterFeatureRepository(
            DB_Shelters context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ShelterFeatureDTO> GetAll()//-----
        {
            return _context.ShelterFeature
                .AsNoTracking()
                .ProjectTo<ShelterFeatureDTO>(
                    _mapper.ConfigurationProvider)
                .ToList();
        }

        public ShelterFeatureDTO? GetById(int id)
        {
            return _context.ShelterFeature
                .Where(p => p.ShelterFeatureId == id)
                .ProjectTo<ShelterFeatureDTO>(
                    _mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public ShelterFeatureDTO Add(ShelterFeatureDTO dto)
        {
            var preference = _mapper.Map<ShelterFeature>(dto);

            _context.ShelterFeature.Add(preference);
            _context.SaveChanges();

            return _mapper.Map<ShelterFeatureDTO>(preference);
        }

        public ShelterFeatureDTO? Update(int id, ShelterFeatureDTO dto)
        {
            var shelterFeature = _context.ShelterFeature
                .FirstOrDefault(sf => sf.ShelterFeatureId == id);

            if (shelterFeature == null)
                return null;

            shelterFeature.IsAvailable = dto.IsAvailable;

            _context.SaveChanges();

            return _mapper.Map<ShelterFeatureDTO>(shelterFeature);
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
