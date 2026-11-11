using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ChatMessageDTO
    {
        //מי כתב את ההודעה (user, assistant)
        public string Role { get; set; } = "";
        public string Text { get; set; } = "";
    }
}
