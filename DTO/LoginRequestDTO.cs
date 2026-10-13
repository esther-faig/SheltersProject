using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LoginRequestDTO
    {
        //לאפשר למשתמשים להתחבר לחשבון קים
        //פה צד הלקוח שולח את כל הנתונים שדרושים להתחברות ולא את כל נתוני המשתמש
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
