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
        //הלקוח שולח את טופס ההרשמה לשרת
        [MaxLength(100),Required]
        public string FullName { get; set; } = "";
        [EmailAddress,Required]
        public string Email { get; set; } = "";
        [MinLength(6),Required]
        public string Password { get; set; } = "";

        //אין ROLE אלא הוא תמיד נקבע מהשרת
    }
}
