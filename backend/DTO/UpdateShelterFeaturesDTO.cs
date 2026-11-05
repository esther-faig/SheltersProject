using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class UpdateShelterFeaturesDTO
    {
        // רק המאפיינים שהמשתמשת רוצה לתקן (PreferenceId + האם קיים במקלט)
        [MinLength(1)]
        public List<ShelterFeatureUpdateDTO> Features { get; set; } = new();
    }
}
