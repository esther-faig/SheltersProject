using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class LoginRequestDTO
    {
        //פה צד הלקוח שולח את כל הנתונים שדרושים להתחברות ולא את כל נתוני המשתמש
        [Required(ErrorMessage = "יש להזין כתובת מייל")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "יש להזין סיסמה")]
        public string Password { get; set; } = "";
    }
}