using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UpdateUserRequestDTO
    {
        // פרטי המשתמש
        [MaxLength(100)]
        public string FullName { get; set; } = "";
        [EmailAddress]
        public string Email { get; set; } = "";

        // ההעדפות שהמשתמש בחר
        public List<UserPreferenceDTO> Preferences { get; set; } = new();
    }
}
