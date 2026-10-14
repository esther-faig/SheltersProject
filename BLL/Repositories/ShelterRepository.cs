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
    public class ShelterRepository: IShelterRepository
    {
        private readonly DB_Shelters _context;
        private readonly IMapper _mapper;

        public ShelterRepository(
            DB_Shelters context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ShelterSearchResultDTO> GetAll()
        {
            return _context.Shelter
                .AsNoTracking()
                .ProjectTo<ShelterSearchResultDTO>(
                    _mapper.ConfigurationProvider)
                .ToList();
        }

        public ShelterSearchResultDTO? GetById(int id)
        {
            return _context.Shelter
                .Where(p => p.ShelterId == id)
                .ProjectTo<ShelterSearchResultDTO>(
                    _mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public ShelterDetailsDTO Add(ShelterRequestDTO dto)
        {
            var shelter = _mapper.Map<Shelter>(dto);

            _context.Shelter.Add(shelter);
            _context.SaveChanges();

            return _mapper.Map<ShelterDetailsDTO>(shelter);
        }

        public ShelterDetailsDTO? Update(int id, ShelterRequestDTO dto)
        {
            var shelter = _context.Shelter
                .FirstOrDefault(sf => sf.ShelterId == id);

            if (shelter == null)
                return null;
            _mapper.Map(dto, shelter);
            _context.SaveChanges();

            _context.SaveChanges();

            return _mapper.Map<ShelterDetailsDTO>(shelter);
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
