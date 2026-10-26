using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserResponseDTO
    {
        //החזרת פרטי משתמש בסיסיים לאחר התחברות/הרשמה.
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Admin, Resident, ShelterManager
        public string Token { get; set; } // JWT Token
    }
}
