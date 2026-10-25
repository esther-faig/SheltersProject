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

        //public ProtectionTime? GetById(int id)
        //{
        //    return _context.ProtectionTime
        //        .FirstOrDefault(pt => pt.ProtectionTimeId == id);
        //}
        //public List<ProtectionTime> GetAll()
        //{
        //    return _context.ProtectionTime
        //        .AsNoTracking()
        //        .ToList();
        //}
        //public List<ProtectionTime> GetByAlertZoneId(int alertZoneId)
        //{
        //    return _context.ProtectionTime
        //        .AsNoTracking()
        //        .Where(pt => pt.AlertZoneId == alertZoneId)
        //        .OrderByDescending(pt => pt.EffectiveFrom)
        //        .ToList();
        //}

        public ProtectionTime Add(ProtectionTime entity)
        {
            _context.ProtectionTime.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public ProtectionTime? GetValidAt(int alertZoneId, string alertType, DateTime atTime)
        {
            return _context.ProtectionTime
                .FirstOrDefault(pt => pt.AlertZoneId == alertZoneId
                                   && pt.AlertType == alertType
                                   && pt.EffectiveFrom <= atTime
                                   && (pt.EffectiveUntil == null || pt.EffectiveUntil > atTime));
        }
        //public bool Delete(int id)
        //{
        //    var entity = _context.ProtectionTime.Find(id);
        //    if (entity != null)
        //    {
        //        _context.ProtectionTime.Remove(entity);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}

        //public ProtectionTime? Update(ProtectionTime entity)
        //{
        //    _context.ProtectionTime.Update(entity);
        //    _context.SaveChanges();
        //    return entity;
        //}
    }
}
