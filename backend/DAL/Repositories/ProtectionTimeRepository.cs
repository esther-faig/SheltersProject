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
    }
}
