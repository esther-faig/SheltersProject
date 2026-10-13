using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CreateShelterRequestDTO
    {
        //לבדוק האם הוא יכול לשמש גם לעריכה כח בעריכה צריך גם את הקוד של המקלט
        //כי צריך לדעת את מי לשנות
        //יתכן שאפשר להוסיף שדה של קוד שיתמלא רק במקרה של עריכה
        //ואם הוא נותר ריק אז הוא יתמלא ע"י השרת
        //משמש גם לעריכה של מקלט קים
        public string Name { get; set; } = "";
        public string ShelterType { get; set; } = "";
        public string Address { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int MaxCapacity { get; set; }

        public List<ShelterFeatureDTO> Features { get; set; } = new();
    }
}
