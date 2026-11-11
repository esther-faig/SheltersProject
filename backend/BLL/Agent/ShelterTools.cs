using System.ComponentModel;
using BLL.Services;
using DTO;

namespace BLL.Agent
{
    // =========================================================================
    // אובייקטי Record קלים (DTOs) להעברת מידע רזה וממוקד למודל ה-AI
    // =========================================================================

    // תקציר נתוני מקלט בודד להחזרה בחיפוש ראשוני
    public record ShelterSummary(int Id, string Name, string Address, string Type);

    //זמן התגוננות בשניות עבור סוג התרעה מסוים
    public record AlertTime(string AlertType, int Seconds);

    //נתוני אזור התרעה וזמני ההתגוננות השייכים לו
    public record ZoneTimes(string AreaName, List<AlertTime> Times);

    //מחלקת הכלים (Tools) של ה-Agent.
    // מתודות אלו נחשפות למודל השפה (Function Calling) כדי לתקשר עם הדאטהבייס.
    public class ShelterTools
    {
        private readonly IShelterService _shelterService;
        private readonly IAlertZoneService _alertZoneService;

        // הזרקת השירותים הנדרשים מה-BLL
        public ShelterTools(IShelterService shelterService, IAlertZoneService alertZoneService)
        {
            _shelterService = shelterService;
            _alertZoneService = alertZoneService;
        }

        //כלי לחיפוש מקלטים בעיר מסוימת. המודל קורא לזה כשהמשתמש שואל "איפה יש מקלט ב...?"
        [Description("מחפש מקלטים מאושרים ופעילים בעיר. מחזיר מזהה, שם, כתובת וסוג.")]
        public List<ShelterSummary> SearchSheltersByCity([Description("שם העיר, למשל: גבעת שמואל")] string city)
            => _shelterService.SearchByCity(city, 10)
                .Select(s => new ShelterSummary(s.ShelterId, s.Name, s.Address, s.ShelterType))
                .ToList();

        // כלי לקבלת פרטים מלאים על מקלט ממוקד (נגישות, מאפיינים וכו') לפי ID
        [Description("מחזיר את כל הפרטים והמאפיינים של מקלט לפי מזהה: נגישות, מדרגות, אוורור ועוד.")]
        public ShelterSearchResultDTO? GetShelterDetails([Description("מזהה המקלט")] int id)
            => _shelterService.GetById(id);

        // כלי לבדיקת זמן התגוננות ואזור התרעה לפי קואורדינטות גיאוגרפיות
        [Description("מחזיר את אזור ההתרעה של מיקום ואת זמני ההתגוננות שלו בשניות, לפי סוג התרעה.")]
        public ZoneTimes? GetProtectionTime(
            [Description("קו רוחב (Latitude)")] double latitude,
            [Description("קו אורך (Longitude)")] double longitude)
        {
            var zone = _alertZoneService.FindByLocation(latitude, longitude);
            if (zone == null) return null;

            return new ZoneTimes(
                zone.AreaName,
                zone.ProtectionTimes.Select(p => new AlertTime(p.AlertType, p.DurationSeconds)).ToList()
            );
        }
    }
}