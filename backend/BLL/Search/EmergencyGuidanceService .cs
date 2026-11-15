using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Search;
using DTO;

namespace BLL.Services
{

    public class EmergencyGuidanceService : IEmergencyGuidanceService
    {
        public EmergencyGuidanceDTO Get(bool inVehicle)
        {
            var lines = inVehicle
                ? new[]
                {
                    "עצרו בצד הדרך, צאו מהרכב והתרחקו ממנו.",
                    "שכבו על הרצפה עם הידיים על הראש.",
                    "המתינו 10 דקות לפחות."
                }
                : new[]
                {
                    "שכבו על הרצפה עם הידיים על הראש.",
                    "התרחקו ממבנים ומחלונות ככל האפשר.",
                    "המתינו 10 דקות לפחות."
                };

            return new EmergencyGuidanceDTO
            {
                Title = "לא נמצא מקלט מתאים בזמן הקצוב",
                Instructions = string.Join("\n", lines),
                Source = "פיקוד העורף",
                SourceUrl = "https://www.oref.org.il",
                UpdatedAt = null
            };
        }
    }
}
