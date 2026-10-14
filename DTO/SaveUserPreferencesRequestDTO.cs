using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SaveUserPreferencesRequestDTO
    {
        //לשלוח לשרת את כל ההעדפות שהמשתמש בחר בהרשמה או בעריכה
        public List<UserPreferenceDTO> Preferences { get; set; } = new();
    }
}
