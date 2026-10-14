using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories
{
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly DB_Shelters _context;
        private readonly IMapper _mapper;

        public PreferenceRepository(
            DB_Shelters context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<PreferenceDTO> GetAll()
        {
            return _context.Preference
                .AsNoTracking()
                .ProjectTo<PreferenceDTO>(
                    _mapper.ConfigurationProvider)
                .ToList();
        }

        public PreferenceDTO? GetById(int id)
        {
            return _context.Preference
                .Where(p => p.PreferenceId == id)
                .ProjectTo<PreferenceDTO>(
                    _mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public PreferenceDTO Add(PreferenceDTO dto)
        {
            var preference = _mapper.Map<Preference>(dto);

            _context.Preference.Add(preference);
            _context.SaveChanges();

            return _mapper.Map<PreferenceDTO>(preference);
        }

        public PreferenceDTO? Update(int id, PreferenceDTO dto)
        {
            var preference = _context.Preference
                .FirstOrDefault(p => p.PreferenceId == id);

            if (preference == null)
                return null;

            preference.Name = dto.Name;
            preference.Description = dto.Description ?? "";
            preference.Category = dto.Category ?? "";

            _context.SaveChanges();

            return _mapper.Map<PreferenceDTO>(preference);
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
    }
}