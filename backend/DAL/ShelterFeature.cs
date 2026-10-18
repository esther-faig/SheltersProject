using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ShelterFeature
    {
        [Key]
        public int ShelterFeatureId { get; set; }

        public int ShelterId { get; set; }
        public Shelter Shelter { get; set; }

        public int PreferenceId { get; set; }
        public Preference Preference { get; set; }

        public bool? IsAvailable { get; set; }//מאפשר NULL
    }
}
