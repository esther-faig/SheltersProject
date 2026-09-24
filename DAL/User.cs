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

       
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public UserRole Roles { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        //ICollection????
        public List<Shelter> Shelters { get; set; } = new List<Shelter>();
        public List<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();

    }
}

