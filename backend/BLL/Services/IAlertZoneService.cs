using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IAlertZoneService
    {
        //אין DTO

        //הנתונים נכנסים בהתחלה ומשמשים רק לחיפוש ולכן לא צריך כלל הוספה וכו

        // לחיפוש: האזור של המיקום + זמני ההתגוננות שתקפים עכשיו. null = מחוץ לכל אזור
        AlertZone? FindByLocation(double latitude, double longitude);

        // כשצריך רק את הקוד (למשל בהוספת מקלט)
        int? FindZoneIdByLocation(double latitude, double longitude);

        // זמן ההתגוננות (בשניות) של האזור לסוג ההתרעה. null = אין נתון לסוג הזה באזור
        int? GetDurationSeconds(AlertZone zone, string alertType);
    }
}
