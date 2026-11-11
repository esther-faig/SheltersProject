using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ChatRequestDTO
    {
        //הרבה הודעות לשיחה אחת
        public List<ChatMessageDTO> Messages { get; set; } = new();

    }
}
