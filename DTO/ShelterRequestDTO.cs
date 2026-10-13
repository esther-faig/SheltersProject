using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterRequestDTO
    {
        //משמש להוספה ועריכה של מקלטים
        //בעריכה הקוד של המקלט ישלח בבקשה לשרת 
        public string Name { get; set; } = "";
        public string ShelterType { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int MaxCapacity { get; set; }

        // פה נשתמש בDTO שמשנים בו על כל העדפה רק אם קימת
        //ואי אפשר לשנות פה את ההעדפה בעצמה
        public List<ShelterFeatureUpdateDTO> Features { get; set; } = new();
       //מחרוזות שהם הניתוב של הקבצים
        public List<string> Files { get; set; } = new();

    }
}
