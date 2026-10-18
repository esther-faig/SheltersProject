using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterFeatureUpdateDTO
    {
        //לשנות העדפות של מקלט
        public int PreferenceId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
