using AutoMapper;
using DAL;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProtectionTimeService:IProtectionTimeService
    {

        private readonly IProtectionTimeRepository _protectionTimeRepository;
        private readonly IMapper _mapper;

        public ProtectionTimeService(IProtectionTimeRepository protectionTimeRepository, IMapper mapper)
        {
            _protectionTimeRepository = protectionTimeRepository;
            _mapper = mapper;
        }

        public ProtectionTime? GetById(int id)
        {
            return _protectionTimeRepository.GetById(id);
        }

        public List<ProtectionTime> GetAll()
        {
            return _protectionTimeRepository.GetAll();
        }

        public ProtectionTime Add(ProtectionTime entity)
        {
            // לוגיקה עסקית נוספת במידת הצורך לפני הוספה
            return _protectionTimeRepository.Add(entity);
        }

        public ProtectionTime? Update(int id, ProtectionTime entity)
        {
            return _protectionTimeRepository.Update(id, entity);
        }

        public bool Delete(int id)
        {
            return _protectionTimeRepository.Delete(id);
        }

    }
}
