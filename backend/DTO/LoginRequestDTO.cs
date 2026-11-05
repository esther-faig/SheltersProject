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
        //לאפשר למשתמשים להתחבר לחשבון קים
        //פה צד הלקוח שולח את כל הנתונים שדרושים להתחברות ולא את כל נתוני המשתמש
        [Required, EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
