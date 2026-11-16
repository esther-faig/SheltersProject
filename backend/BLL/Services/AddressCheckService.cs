using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Agent;
using BLL.Exceptions;

namespace BLL.Services
{

    public class AddressCheckService : IAddressCheckService
    {
        // הפרש מותר בין מיקום הכתובת לקואורדינטות שהמשתמש סימן
        private const double MaxDistanceMeters = 300;

        private readonly GeocodingTools _geocoding;

        public AddressCheckService(GeocodingTools geocoding)
        {
            _geocoding = geocoding;
        }

        public void EnsureMatches(string? address, string city, double latitude, double longitude)
        {
            // הכתובת לא שדה חובה בשרת, ולכן בלעדיה אין מה לבדוק
            if (string.IsNullOrWhiteSpace(address)) return;

            GeoPoint? found;
            try
            {
                // ShelterService סינכרוני, ולכן ממתינים לתשובה כאן
                found = _geocoding
                    .GeocodeAddressAsync($"{address.Trim()}, {city.Trim()}", CancellationToken.None)
                    .GetAwaiter().GetResult();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                throw new BusinessRuleException("לא הצלחנו לאמת את הכתובת כרגע. נסו שוב בעוד רגע.");
            }

            // אם תרצי לא לחסום כתובות ש-OpenStreetMap לא מכיר, החליפי את ה-throw ב-return
            if (found == null)
                throw new BusinessRuleException("לא מצאנו את הכתובת במפה. בדקו שרחוב, מספר ועיר כתובים נכון.");

            double meters = GeoMath.HaversineMeters(latitude, longitude, found.Latitude, found.Longitude);
            if (meters > MaxDistanceMeters)
                throw new BusinessRuleException(
                    $"הכתובת לא תואמת לקווי האורך והרוחב (הפרש של כ-{Math.Round(meters)} מטר). " +
                    "בדקו את הכתובת, או סמנו מחדש את המיקום על המפה.");
        }
    }
}
