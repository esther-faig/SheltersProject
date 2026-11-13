using BLL.Agent;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Search
{
    public class ShelterSearchOptions
    {
        //כל ההגדרות הנצרכות בביל בהחישובים    

        public const string SectionName = "ShelterSearch";

        // סוג התרעה כשהלקוח לא שלח
        public string DefaultAlertType { get; set; } = "alarm";

        // מרווח ביטחון שמורידים מזמן ההתגוננות (החלטת תכנון, לא נתון רשמי)
        public int SafetyMarginSeconds { get; set; } = 10;

        // כמה מקלטים לכל היותר נשלחים ל-Google
        public int MaxCandidatesForRouting { get; set; } = 50;

        // כמה תוצאות מחזירים (מומלץ + חלופות)
        public int MaxResults { get; set; } = 4;

        // משקלי הציון
        public double PreferenceWeight { get; set; } = 0.7;
        public double TravelTimeWeight { get; set; } = 0.3;

        // true: מאפיין חובה שלא ידוע (NULL / אין רשומה) פוסל את המקלט
        public bool UnknownFailsRequired { get; set; } = true;

        // מהירויות "נדיבות" - רק לפסילה בטוחה בשלב ה-SQL (מטר/שנייה)
        public double MaxWalkSpeedMps { get; set; } = 3.0;
        public double MaxDriveSpeedMps { get; set; } = 35.0;

        // מהירויות ריאליות - רק להערכה כש-Google לא זמין
        public double EstimatedWalkSpeedMps { get; set; } = 1.4;
        public double EstimatedDriveSpeedMps { get; set; } = 8.0;
        public double StraightLineFactor { get; set; } = 1.3;

        // Google
        public string GoogleApiKey { get; set; } = "";
    }
}

