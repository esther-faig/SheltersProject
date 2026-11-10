using System.ComponentModel.DataAnnotations;
using DTO.Validation;
using Microsoft.AspNetCore.Http;

namespace DTO
{
    public class ShelterRequestDTO
    {
        //משמש להוספה ועריכה של מקלטים
        [Required(ErrorMessage = "יש להזין שם למקלט")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם המקלט חייב להכיל בין 2 ל-100 תווים")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "יש לבחור סוג מקלט")]
        [StringLength(50, ErrorMessage = "סוג המקלט ארוך מדי")]
        public string ShelterType { get; set; } = "";

        // בישות ה-DAL הכתובת לא חובה (string?), לכן גם כאן
        [StringLength(200, ErrorMessage = "הכתובת ארוכה מדי")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "יש להזין עיר")]
        [StringLength(100, ErrorMessage = "שם העיר ארוך מדי")]
        public string City { get; set; } = "";

        [Range(-90, 90, ErrorMessage = "קו רוחב חייב להיות בין -90 ל-90")]
        public double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "קו אורך חייב להיות בין -180 ל-180")]
        public double Longitude { get; set; }

        [Range(1, 100000, ErrorMessage = "קיבולת המקלט חייבת להיות בין 1 ל-100000")]
        public int MaxCapacity { get; set; }

        // כל פריט ברשימה נבדק אוטומטית לפי ה-Attributes שב-ShelterFeatureUpdateDTO
        public List<ShelterFeatureUpdateDTO> Features { get; set; } = new();

        // תמונות חדשות שהמשתמש מעלה (לא חובה)
        [AllowedExtensions(".jpg", ".jpeg", ".png", ".webp")]
        [MaxFileSize(5)]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

        // בעריכה: שמות של תמונות קיימות שרוצים לשמור (מה שלא נשלח יימחק)
        public List<string> ExistingFiles { get; set; } = new();
    }
}