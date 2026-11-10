using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    // הפרה של כלל במערכת: העדפה שלא קיימת, דחייה בלי סיבה, יותר מדי תמונות  (ה-Filter מחזיר 400)
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
