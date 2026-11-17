using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AlertServerOptions
    {
        public const string SectionName = "AlertServer";

        // תאימות לאחור: אם אין BaseUrls משתמשים ב-BaseUrl הישן
        public string? BaseUrl { get; set; }
        public string[] BaseUrls { get; set; } = Array.Empty<string>();

        public int PollIntervalSeconds { get; set; } = 2;
        public int TimeoutSeconds { get; set; } = 3;

        // כמה שניות התרעה נשארת "חיה" אחרי שנעלמה מהשרת. 0 = נעלמת מיד (נוח לבדיקות עם המדומה)
        public int RetentionSeconds { get; set; } = 0;

        // אם אין קריאה מוצלחת כל כך הרבה זמן, לא סומכים על התרעה ישנה
        public int StaleAfterSeconds { get; set; } = 10;

        public IReadOnlyList<string> ResolveUrls()
        {
            var urls = BaseUrls.Where(u => !string.IsNullOrWhiteSpace(u)).ToList();
            if (urls.Count == 0 && !string.IsNullOrWhiteSpace(BaseUrl)) urls.Add(BaseUrl);
            if (urls.Count == 0) urls.Add("https://www.oref.org.il/");

            // חובה לוכסן בסוף, אחרת הנתיב היחסי נחתך
            return urls.Select(u => u.EndsWith('/') ? u : u + "/").ToList();
        }
    }
}