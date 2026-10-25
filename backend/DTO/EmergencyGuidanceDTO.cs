using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EmergencyGuidanceDTO
    {
        //אם לא נמצא מקלט מתאים, מסך התוצאות אמור להציג הודעה והנחיות חירום רשמיות.
        public string Title { get; set; }
        public string Instructions { get; set; }
        public string Source { get; set; }
        public string SourceUrl { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
