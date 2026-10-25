using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public enum UserRole
    {
        Admin,
        Resident,
        ShelterManager
    }

    public class User
    {

       
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public List<Shelter> Shelters { get; set; } = new List<Shelter>();
        public List<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();

    }
}

