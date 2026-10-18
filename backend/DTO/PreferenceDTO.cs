using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PreferenceDTO
    {
        //השרת שולח את רשימת ההעדפות שיש במערכת להצגה לבחירה למשתמש
        public int PreferenceId { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}
