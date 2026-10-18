using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using DAL.Repositories;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ShelterService : IShelterService
    {
        private readonly IShelterRepository _repository;
        private readonly IMapper _mapper;

        public ShelterService(IShelterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<ShelterSearchResultDTO> GetAll()
        {
            var entities = _repository.GetAll();
            return _mapper.Map<List<ShelterSearchResultDTO>>(entities);
        }

        public ShelterSearchResultDTO? GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : _mapper.Map<ShelterSearchResultDTO>(entity);
        }

        public ShelterDetailsDTO Add(int userId, ShelterRequestDTO dto)
        {
            var entity = _mapper.Map<DAL.Shelter>(dto);
            entity.CreatedByUserId = userId;   // המשתמש ששיתף את המקלט

            var added = _repository.Add(entity);
            return _mapper.Map<ShelterDetailsDTO>(added);
        }
        public ShelterDetailsDTO? Update(int id, ShelterRequestDTO dto)
        {
            var entity = _mapper.Map<DAL.Shelter>(dto);
            var updated = _repository.Update(id, entity);
            return updated == null ? null : _mapper.Map<ShelterDetailsDTO>(updated);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}
