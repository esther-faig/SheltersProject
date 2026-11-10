using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    // המבוקש לא נמצא: מקלט, משתמש, העדפה וכדומה  (ה-Filter מחזיר 404)
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
