using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace DAL.Repositories
{
    public class AlertZoneRepository :IAlertZoneRepository
    {
        private readonly DB_Shelters _context;

        public AlertZoneRepository(DB_Shelters context)
        {
            _context = context;
        }
        public AlertZone? GetContainingPoint(Point point, DateTime atTime)
        {
            return _context.AlertZone
                .AsNoTracking()
                .Include(az => az.ProtectionTimes.Where(pt =>
                    pt.EffectiveFrom <= atTime &&
                    (pt.EffectiveUntil == null || pt.EffectiveUntil > atTime)))
                .FirstOrDefault(az => az.AreaGeometry.Contains(point));
        }


        //public AlertZone? GetById(int id)
        //{

        //    return _context.AlertZone
        //        .Include(az => az.ProtectionTimes)
        //        .FirstOrDefault(az => az.AlertZoneId == id); ;

        //}
        //// בלי Include: הגיאומטריה של כל האזורים כבר כבדה, ורשימות לא צריכות את הזמנים
        //public List<AlertZone> GetAll()
        //{
        //    return _context.AlertZone
        //                    .AsNoTracking()
        //                    .ToList();
        //}
        //public AlertZone? GetByOfficialCode(string officialCode)
        //{
        //    return _context.AlertZone
        //        .AsNoTracking()
        //        .FirstOrDefault(az => az.OfficialCode == officialCode);
        //}
        //// שאילתת החיפוש: האזור של המשתמש + זמני ההתגוננות שתקפים עכשיו, בשאילתה אחת
        //public AlertZone? GetContainingPoint(Point point, DateTime atTime)
        //{
        //    return _context.AlertZone
        //        .AsNoTracking()
        //        .Include(az => az.ProtectionTimes.Where(pt =>
        //            pt.EffectiveFrom <= atTime &&
        //            (pt.EffectiveUntil == null || pt.EffectiveUntil > atTime)))
        //        .FirstOrDefault(az => az.AreaGeometry.Contains(point));
        //}
        //public AlertZone Add(AlertZone entity)
        //{
        //    _context.AlertZone.Add(entity);
        //    _context.SaveChanges();
        //    return entity;
        //}

        //public AlertZone? Update(AlertZone entity)
        //{
        //    _context.AlertZone.Update(entity);
        //    _context.SaveChanges();
        //    return entity;
        //}
        //public bool Delete(int id)
        //{
        //    var entity = _context.AlertZone.Find(id);
        //    if (entity != null)
        //    {
        //        _context.AlertZone.Remove(entity);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}

       
    }
}
