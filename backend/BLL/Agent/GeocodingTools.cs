using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace BLL.Agent
{
    /// <summary>
    /// ייצוג נתונים גיאוגרפי (Record) הכולל קו רוחב, קו אורך וכתובת מלאה מתואמת.
    /// </summary>
    public record GeoPoint(double Latitude, double Longitude, string FormattedAddress);

    /// <summary>
    /// מחלקה עזר למיפוי (Mapping) התשובה המתקבלת מ-API של Nominatim (OpenStreetMap).
    /// שים לב: Nominatim מחזיר מערך של תוצאות, ואת ערכי ה-lat וה-lon הוא שולח כמחרוזות.
    /// </summary>
    public class NominatimResult
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; } = "";

        [JsonPropertyName("lon")]
        public string Lon { get; set; } = "";

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = "";
    }

    /// <summary>
    /// מחלקת כלים לביצוע פעולות Geocoding (המרת כתובות טקסטואליות לקואורדינטות גיאוגרפיות).
    /// </summary>
    public class GeocodingTools
    {
        // מופע של HttpClient לשליחת בקשות HTTP לשרת החיצוני
        private readonly HttpClient _http;

        /// <summary>
        /// בנאי המקבל HttpClient באמצעות Dependency Injection.
        /// </summary>
        public GeocodingTools(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// ממיר כתובת טקסטואלית בישראל לקואורדינטות (קו רוחב וקו אורך). 
        /// </summary>
        /// <param name="address">כתובת מלאה לחיפוש, למשל: הרצל 10 גבעת שמואל</param>
        /// <param name="ct">אסימון ביטול (CancellationToken) להפסקה מבוקרת של הבקשה</param>
        /// <returns>אובייקט GeoPoint המכיל את הנתונים, או null אם הכתובת לא נמצאה</returns>
        [Description("ממיר כתובת בישראל לקואורדינטות (קו רוחב וקו אורך). מחזיר null אם הכתובת לא נמצאה.")]
        public async Task<GeoPoint?> GeocodeAddressAsync(
            [Description("כתובת מלאה, למשל: הרצל 10 גבעת שמואל")] string address,
            CancellationToken ct)
        {
            // בניית כתובת ה-URL לשאילתה מול Nominatim:
            // - EscapeDataString מטפל בתווים מיוחדים ורווחים בכתובת.
            // - format=jsonv2 מבקש מבנה JSON מעודכן.
            // - limit=1 מגביל לתוצאה הרלוונטית ביותר.
            // - countrycodes=il מגביל את החיפוש לישראל בלבד.
            // - accept-language=he מבקש תוצאות בשפה העברית.
            var url = $"search?q={Uri.EscapeDataString(address)}&format=jsonv2&limit=1&countrycodes=il&accept-language=he";

            // שליחת בקשת GET ופענוח אוטומטי של רשימת התוצאות מ-JSON
            var results = await _http.GetFromJsonAsync<List<NominatimResult>>(url, ct);

            // שליפת התוצאה הראשונה מהרשימה (אם קיימת)
            var first = results?.FirstOrDefault();
            if (first == null) return null;

            // יצירת והחזרת אובייקט ה-GeoPoint:
            // המרת המחרוזות של קו הרוחב והאורך למספרים עשרוניים (double) תוך שימוש ב-InvariantCulture,
            // כדי להבטיח שהנקודה העשרונית מנוסחת נכון ללא תלות בהגדרות האזוריות (Culture) של השרת.
            return new GeoPoint(
                double.Parse(first.Lat, CultureInfo.InvariantCulture),
                double.Parse(first.Lon, CultureInfo.InvariantCulture),
                first.DisplayName);
        }
    }
}