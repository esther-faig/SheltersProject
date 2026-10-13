using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AdminShelterDTO
    {
        //הצגת פרטי מקלט מלאים למנהל המערכת
        public int ShelterId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string ShelterType { get; set; } = "";
        public string Status { get; set; } = "";
        //מאפיני המקלט
        public List<ShelterFeatureDTO> Features { get; set; }
        public List<string> Files { get; set; } = new();
    }
}
