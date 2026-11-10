using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LoginRequestDTO
    {
        // לאפשר למשתמשים להתחבר לחשבון קיים
        // פה צד הלקוח שולח את כל הנתונים שדרושים להתחברות ולא את כל נתוני המשתמש

        [Required(ErrorMessage = "יש להזין כתובת מייל")]
        [EmailAddress(ErrorMessage = "כתובת המייל אינה תקינה")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "יש להזין סיסמה")]
        public string Password { get; set; } = "";
    }
}
