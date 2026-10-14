using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SharedShelterDTO
    {
        //צד שרת שולח את המקלטים שהמשתמש שיתף להציג אותם בפרופיל שלו
        public int ShelterId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string ShelterType { get; set; } = "";
        public string Status { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
