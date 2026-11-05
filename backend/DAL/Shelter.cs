using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;  


namespace DAL
{
    public enum ShelterStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Shelter
    {
        public int ShelterId { get; set; }

        public string Name { get; set; }

        public string ShelterType { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int MaxCapacity { get; set; }

        public ShelterStatus Status { get; set; } = ShelterStatus.Pending;
        public bool IsActive { get; set; }= true; // false = בעל המקלט ביטל אותו זמנית. מקלט לא פעיל לא מוצג בחיפוש בכלל
        // סיבת הדחייה (אם המנהל דחה את המקלט)
        [MaxLength(500)]
        public string? RejectionReason { get; set; }

        // true = מקלט שנטען מראש מנתונים רשמיים (עיריות וכו').
        // רק במקלטים כאלה כל משתמשת מחוברת יכולה לתקן מאפיינים.
        // ברירת מחדל false: מקלט ששיתפה משתמשת נעול לכל מי שאינה הבעלים
        public bool IsImported { get; set; } = false;
        public int? AlertZoneId { get; set; } // FK לאזור ההתרעה

        //מאפשר את טעינת האוביקט המקושר בשאילתה פשוטה במקום שנצטרך לחפש את האיזור בטבלה לפי הקוד שלו
        public AlertZone? AlertZone { get; set; } // ניווט לטבלת AlertZone

        public int CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public User User { get; set; } = null!;


        public List<string> Files { get; set; } = new List<string>();
        public List<ShelterFeature> ShelterFeatures { get; set; } = new List<ShelterFeature>();


     
    }
}
