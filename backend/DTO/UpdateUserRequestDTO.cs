using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UpdateUserRequestDTO
    {
        // פרטי המשתמש
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";

        // ההעדפות שהמשתמש בחר
        public List<UserPreferenceDTO> Preferences { get; set; } = new();
    }
}
