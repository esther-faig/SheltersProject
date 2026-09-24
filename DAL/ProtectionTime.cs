using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProtectionTime
    {
        [Key]
        public int ProtectionTimeId { get; set; }

        public int AlertZoneId { get; set; }

        public string AlertType { get; set; }

        public int DurationSeconds { get; set; }

        // ממתי הזמן הזה תקף
        public DateTime EffectiveFrom { get; set; }

        // עד מתי הוא תקף (null במידה ועדיין בתוקף)
        public DateTime? EffectiveUntil { get; set; }

        public string Source { get; set; }

        public DateTime UpdatedAt { get; set; }

        // ניווט לטבלת AlertZone
        public AlertZone AlertZone { get; set; } = null!;



    }
}
