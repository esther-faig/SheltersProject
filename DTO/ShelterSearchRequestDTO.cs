using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterSearchRequestDTO
    {
        //צד לקוח שולח לשרת את הנתונים הדרושים לו לחיפוש
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TravelMode { get; set; } = "WALKING";//ברירת מחדל
    }
}
