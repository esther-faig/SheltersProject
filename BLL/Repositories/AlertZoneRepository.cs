using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class AlertZoneRepository :IAlertZoneRepository
    {
        //אין DTO
        private readonly DB_Shelters _context;
        //private readonly IMapper _mapper;

        public AlertZoneRepository(DB_Shelters context)
        {
            _context = context;
            //_mapper = mapper;
        }

        public AlertZone GetById(int id)
        {
            return _context.AlertZone
            .Include(az => az.ProtectionTimes) 
            .Include(az => az.Shelters)
            .FirstOrDefault(az => az.AlertZoneId == id);
        }
        public List<AlertZone> GetAll()
        {
            return _context.AlertZone
         .Include(az => az.ProtectionTimes) // טעינת זמני ההתגוננות הקשורים
         .Include(az => az.Shelters)         // טעינת המקלטים הקשורים
         .ToList();
        }
        public AlertZone Add(AlertZone entity)
        {
            _context.AlertZone.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public bool Delete(int id)
        {
            var entity = _context.AlertZone.Find(id);
            if (entity != null)
            {
                _context.AlertZone.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        // 5. עדכון אובייקט
        public AlertZone Update(int id,AlertZone entity)
        {

            if (entity == null)
            {
                return null;
            }

            // עדכון האובייקט ושמירת השינויים במסד הנתונים
            entity.AlertZoneId = id;
            _context.AlertZone.Update(entity);
            _context.SaveChanges();

            // שליפת האובייקט המעודכן מחדש כולל קשרי הגומלין באמצעות Include
            return _context.AlertZone
                .Include(az => az.ProtectionTimes) // טעינת זמני ההתגוננות הקשורים
                .Include(az => az.Shelters)         // טעינת המקלטים הקשורים
                .FirstOrDefault(az => az.AlertZoneId == id);
        }
    }
}
