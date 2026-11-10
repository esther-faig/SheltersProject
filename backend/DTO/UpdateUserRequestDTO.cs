using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class UpdateUserRequestDTO
    {
        // פרטי המשתמש
        [Required(ErrorMessage = "יש להזין שם")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "השם חייב להכיל בין 2 ל-100 תווים")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "יש להזין כתובת מייל")]
        [EmailAddress(ErrorMessage = "כתובת המייל אינה תקינה")]
        [StringLength(256, ErrorMessage = "כתובת המייל ארוכה מדי")]
        public string Email { get; set; } = "";

        // ההעדפות שהמשתמש בחר
        public List<UserPreferenceDTO> Preferences { get; set; } = new();
    }
}