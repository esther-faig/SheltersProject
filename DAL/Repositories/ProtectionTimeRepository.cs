using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProtectionTimeRepository: IProtectionTimeRepository
    {

        private readonly DB_Shelters _context;

        public ProtectionTimeRepository(DB_Shelters context)
        {
            _context = context;
        }

        public ProtectionTime? GetById(int id)
        {
            return _context.ProtectionTime
                .Include(pt => pt.AlertZone)
                .FirstOrDefault(pt => pt.ProtectionTimeId == id);
        }
        public List<ProtectionTime> GetAll()
        {
            return _context.ProtectionTime
                .Include(pt => pt.AlertZone)
                .ToList();
        }
        public ProtectionTime Add(ProtectionTime entity)
        {
            _context.ProtectionTime.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public bool Delete(int id)
        {
            var entity = _context.ProtectionTime.Find(id);
            if (entity != null)
            {
                _context.ProtectionTime.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public ProtectionTime? Update(int id, ProtectionTime entity)
        {

            if (entity == null)
            {
                return null;
            }

            // עדכון האובייקט ושמירת השינויים במסד הנתונים
            entity.ProtectionTimeId = id;
            _context.ProtectionTime.Update(entity);
            _context.SaveChanges();



            // שליפת האובייקט המעודכן מחדש כולל קשרי הגומלין באמצעות Include
            return _context.ProtectionTime
           .Include(pt => pt.AlertZone)
           .FirstOrDefault(pt => pt.ProtectionTimeId == id);
        }
    }
}
