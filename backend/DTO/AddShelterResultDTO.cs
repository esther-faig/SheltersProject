using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    // תשובת שיתוף מקלט. Saved=false זו אזהרה ולא שגיאה: שום דבר לא נשמר עד שהמשתמש מאשר
    public class AddShelterResultDTO
    {
        public bool Saved { get; set; }

        // מלא רק כש-Saved = true
        public ShelterDetailsDTO? Shelter { get; set; }

        // מלא רק כשיש אזהרה
        public string? Message { get; set; }
        public List<NearbyShelterDTO> NearbyShelters { get; set; } = new();
    }

    public class NearbyShelterDTO
    {
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string ShelterType { get; set; } = "";
        public double DistanceMeters { get; set; }

        // true = מקלט ציבורי או של המשתמש עצמו (מותר להציג שם וכתובת)
        public bool DetailsVisible { get; set; }
    }
}
