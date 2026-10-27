using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DTO
{
    public class ShelterDetailsDTO
    {

        //הצגת פרטי מקלט מלאים
        public int ShelterId { get; set; }
        public string Name { get; set; } = "";
        public string ShelterType { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int MaxCapacity { get; set; }
        

        public List<ShelterFeatureDTO> Features { get; set; } = new();

        //משמש להצגה לקלינט ולכן נשמר פה רק השם
        public List<string> ImageFiles { get; set; } = new();
    }
}
