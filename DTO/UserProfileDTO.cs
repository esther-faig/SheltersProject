using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserProfileDTO
    {
        //הצגת הפרופיל של המשתמש כולל מקלטים ששיתף
        public int UserId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";

        public List<UserPreferenceDTO> Preferences { get; set; }
        public List<SharedShelterDTO> SharedShelters { get; set; } 
    }
}
