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
        public AlertZone? GetByOfficialCode(string officialCode)
        {
            return _context.AlertZone
                .AsNoTracking()
                .FirstOrDefault(az => az.OfficialCode == officialCode);
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

       
    }
}
