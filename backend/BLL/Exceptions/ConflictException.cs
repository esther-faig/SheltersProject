using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    // התנגשות עם מה שכבר קיים: מייל רשום, שם העדפה כפול, העדפה בשימוש  (ה-Filter מחזיר 409)
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
