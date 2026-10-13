using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserPreferenceDTO
    {
        //משמש להצגת ההעדפות של המשתמש וגם לשליחת העדפותיו לשמירה
        public int PreferenceId { get; set; }
        public string Name { get; set; } = "";
        public string? Category { get; set; }
        public int Priority { get; set; }
        public bool IsRequired { get; set; }
    }
}
