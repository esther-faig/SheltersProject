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

        private readonly IProtectionTimeRepository _repository ;

        public ProtectionTimeService(IProtectionTimeRepository repository)
        {
            _repository = repository;
        }

        public ProtectionTime SetDuration(int alertZoneId, string alertType, int durationSeconds, string source)
        {
            var now = DateTime.UtcNow;
            var current = _repository.GetValidAt(alertZoneId, alertType, now);

            // אותו ערך כבר בתוקף, אין מה לעשות
            if (current != null && current.DurationSeconds == durationSeconds)
                return current;

            // סוגרים את הרשומה הישנה. היא נשמרת באותו SaveChanges של החדשה
            if (current != null)
                current.EffectiveUntil = now;

            var entity = new ProtectionTime
            {
                AlertZoneId = alertZoneId,
                AlertType = alertType,
                DurationSeconds = durationSeconds,
                Source = source,
                EffectiveFrom = now,
                EffectiveUntil = null,
                UpdatedAt = now
            };

            return _repository.Add(entity);
        }

    }
}
