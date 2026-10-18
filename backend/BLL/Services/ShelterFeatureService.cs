using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.Repositories;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ShelterFeatureService : IShelterFeatureService
    {
        private readonly IShelterFeatureRepository _repository;
        private readonly IMapper _mapper;

        public ShelterFeatureService(IShelterFeatureRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<ShelterFeatureDTO> GetAll()
        {
            var entities = _repository.GetAll();
            return _mapper.Map<List<ShelterFeatureDTO>>(entities);
        }

        public ShelterFeatureDTO? GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : _mapper.Map<ShelterFeatureDTO>(entity);
        }

        public ShelterFeatureDTO Add(ShelterFeatureDTO dto)
        {
            var entity = _mapper.Map<DAL.ShelterFeature>(dto);
            var added = _repository.Add(entity);
            return _mapper.Map<ShelterFeatureDTO>(added);
        }

        public ShelterFeatureDTO? Update(int id, ShelterFeatureDTO dto)
        {
            var entity = _mapper.Map<DAL.ShelterFeature>(dto);
            var updated = _repository.Update(id, entity);
            return updated == null ? null : _mapper.Map<ShelterFeatureDTO>(updated);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}
