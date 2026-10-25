using AutoMapper;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace BLL.Services
{
    public class AlertZoneService : IAlertZoneService
    {
        private readonly IAlertZoneRepository _repository;

        // SRID 4326 = קואורדינטות GPS רגילות (WGS84), מתאים לעמודת geography
        private static readonly GeometryFactory Factory = new GeometryFactory(new PrecisionModel(), 4326);

        public AlertZoneService(IAlertZoneRepository repository)
        {
            _repository = repository;
        }

        public AlertZone? FindByLocation(double latitude, double longitude)
        {
            // ב-NetTopologySuite X = קו אורך, Y = קו רוחב 
            var point = Factory.CreatePoint(new Coordinate(longitude, latitude));

            // "עכשיו" נקבע כאן, ב-Service, ולא ב-Repository
            return _repository.GetContainingPoint(point, DateTime.UtcNow);
        }

        public int? FindZoneIdByLocation(double latitude, double longitude)
        {
            var point = Factory.CreatePoint(new Coordinate(longitude, latitude));
            return _repository.GetContainingPoint(point, DateTime.UtcNow)?.AlertZoneId;
        }

        public int? GetDurationSeconds(AlertZone zone, string alertType)
        {
            // הרשומות שנטענו כבר מסוננות לתקפות עכשיו. אם בכל זאת יש כמה, הכי חדשה קובעת
            return zone.ProtectionTimes
                .Where(pt => string.Equals(pt.AlertType, alertType, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(pt => pt.EffectiveFrom)
                .Select(pt => (int?)pt.DurationSeconds)
                .FirstOrDefault();
        }
    }
}
