using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class PreferenceDTO
    {
        //השרת שולח את רשימת ההעדפות שיש במערכת, והמנהל שולח העדפה חדשה/מעודכנת
        public int PreferenceId { get; set; }

        // המגבלות זהות לעמודות במסד (100 / 500 / 100), אחרת חריגה במסד מחזירה 500
        [Required(ErrorMessage = "יש להזין שם להעדפה")]
        [StringLength(100, ErrorMessage = "שם ההעדפה ארוך מדי (עד 100 תווים)")]
        public string Name { get; set; } = "";

        [StringLength(500, ErrorMessage = "התיאור ארוך מדי (עד 500 תווים)")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "הקטגוריה ארוכה מדי (עד 100 תווים)")]
        public string? Category { get; set; }
    }
}