
using BLL.Agent;
using DAL;

namespace BLL.Search
{
    public class ShelterSearchOptions
    {
        public const string SectionName = "ShelterSearch";

        // סוג ההתרעה שבו משתמשים כשאין התרעה פעילה משרת הדמה
        public string DefaultAlertType { get; set; } = "Alarm";

        // מרווח ביטחון שמורידים מזמן ההתגוננות
        public int SafetyMarginSeconds { get; set; } = 8;

        // מספר המקלטים המרבי שנשלח לחישוב מסלולים
        public int MaxCandidatesForRouting { get; set; } = 50;

        // מספר התוצאות המרבי שמחזירים למשתמש
        public int MaxResults { get; set; } = 4;

        // משקלי הציון
        public double PreferenceWeight { get; set; } = 0.7;
        public double TravelTimeWeight { get; set; } = 0.3;

        // מהירויות מרביות לסינון ראשוני (מטר לשנייה)
        public double MaxWalkSpeedMps { get; set; } = 3.0;
        public double MaxDriveSpeedMps { get; set; } = 35.0;

        // מהירויות להערכה במקרה שאין נתוני מסלול מ-Google
        public double EstimatedWalkSpeedMps { get; set; } = 1.4;
        public double EstimatedDriveSpeedMps { get; set; } = 8.0;
        public double StraightLineFactor { get; set; } = 1.3;

        // מפתח Google
        public string GoogleApiKey { get; set; } = "";
    }
}
