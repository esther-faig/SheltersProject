using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public static class GeoMath
    {
        private const double EarthRadiusMeters = 6371000;

        // מרחק בקו אווירי (Haversine)
        public static double HaversineMeters(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = ToRad(lat2 - lat1);
            double dLon = ToRad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * EarthRadiusMeters * Math.Asin(Math.Sqrt(a));
        }

        // ריבוע שמכיל את כל העיגול ברדיוס נתון. פסילה בטוחה: מה שמחוץ לריבוע בוודאות רחוק מדי
        public static (double MinLat, double MaxLat, double MinLon, double MaxLon) BoundingBox(
            double lat, double lon, double radiusMeters)
        {
            double dLat = radiusMeters / 111_320.0;
            double dLon = radiusMeters / (111_320.0 * Math.Cos(ToRad(lat)));
            return (lat - dLat, lat + dLat, lon - dLon, lon + dLon);
        }

        private static double ToRad(double deg) => deg * Math.PI / 180.0;
    }
}
