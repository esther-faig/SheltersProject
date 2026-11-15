using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterSearchResponseDTO
    {
        public List<ShelterSearchResultDTO> Results { get; set; } = new();
        public bool HasResults { get; set; }
        public string Message { get; set; } = "";

        public string AlertType { get; set; } = "";
        public int MaxSecondsToReach { get; set; }          // הזמן המרבי להגעה אחרי מרווח הביטחון

        // true כשהחיפוש היה ברגל: הלקוח מציע "חפש מקלטים רחוקים יותר, אני ברכב"
        public bool OfferDrivingSearch { get; set; }

        // מלא כשאין תוצאות
        public EmergencyGuidanceDTO? EmergencyGuidance { get; set; }
    }
}
