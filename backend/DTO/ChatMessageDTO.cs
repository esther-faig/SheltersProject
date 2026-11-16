using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class ChatMessageDTO
    {
        //מי כתב את ההודעה (user, assistant). לא מאפשרים system מהלקוח
        [RegularExpression("^(user|assistant)$", ErrorMessage = "תפקיד לא תקין")]
        public string Role { get; set; } = "";

        [StringLength(1000, ErrorMessage = "ההודעה ארוכה מדי (עד 1000 תווים)")]
        public string Text { get; set; } = "";
    }
}