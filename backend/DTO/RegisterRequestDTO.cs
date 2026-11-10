using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class RegisterRequestDTO
    {
        // הלקוח שולח את טופס ההרשמה לשרת
        [Required(ErrorMessage = "יש להזין שם")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "השם חייב להכיל בין 2 ל-100 תווים")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "יש להזין כתובת מייל")]
        [EmailAddress(ErrorMessage = "כתובת המייל אינה תקינה")]
        [StringLength(256, ErrorMessage = "כתובת המייל ארוכה מדי")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "יש להזין סיסמה")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "הסיסמה חייבת להכיל בין 6 ל-100 תווים")]
        public string Password { get; set; } = "";

        // אין ROLE אלא הוא תמיד נקבע מהשרת
    }
}
