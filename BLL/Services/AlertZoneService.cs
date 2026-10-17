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
    public class AlertZoneService:IAlertZoneService
    {
       
        
        private readonly IAlertZoneRepository _alertZoneRepository;
        private readonly IMapper _mapper;

        public AlertZoneService(IAlertZoneRepository alertZoneRepository, IMapper mapper)
        {
            _alertZoneRepository = alertZoneRepository;
            _mapper = mapper;
        }

        public AlertZone? GetById(int id)
        {
            return _alertZoneRepository.GetById(id);
        }

        public List<AlertZone> GetAll()
        {
            return _alertZoneRepository.GetAll();
        }

        public AlertZone Add(AlertZone entity)
        {
            // לוגיקה עסקית נוספת במידת הצורך לפני הוספה
            return _alertZoneRepository.Add(entity);
        }

        public AlertZone? Update(int id, AlertZone entity)
        {
            return _alertZoneRepository.Update(id, entity);
        }

        public bool Delete(int id)
        {
            return _alertZoneRepository.Delete(id);
        }
        

    }
}
