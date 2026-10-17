using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.Repositories;
using DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IPreferenceRepository _repository;
        private readonly IMapper _mapper;

        public PreferenceService(IPreferenceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<PreferenceDTO> GetAll()
        {
            var entities = _repository.GetAll();
            return _mapper.Map<List<PreferenceDTO>>(entities);
        }

        public PreferenceDTO? GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : _mapper.Map<PreferenceDTO>(entity);
        }

        public PreferenceDTO Add(PreferenceDTO dto)
        {
            var entity = _mapper.Map<DAL.Preference>(dto);
            var added = _repository.Add(entity);
            return _mapper.Map<PreferenceDTO>(added);
        }

        public PreferenceDTO? Update(int id, PreferenceDTO dto)
        {
            var entity = _mapper.Map<DAL.Preference>(dto);
            var updated = _repository.Update(id, entity);
            return updated == null ? null : _mapper.Map<PreferenceDTO>(updated);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}

