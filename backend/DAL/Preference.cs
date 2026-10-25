using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Preference
    {

        public int PreferenceId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }

        // Navigation Properties (קשרי רבים-לרבים באמצעות טבלאות קישור)
        public List<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();
        public List<ShelterFeature> ShelterFeatures { get; set; } = new List<ShelterFeature>();
        
    }
}
