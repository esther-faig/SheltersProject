using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterFeatureDTO
    {
        //השרת שולח את המאפינים והאם הם קימים במקלט
        public int PreferenceId { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool IsAvailable { get; set; }
    }
}
