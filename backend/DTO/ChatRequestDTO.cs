using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class ChatRequestDTO
    {
        //הרבה הודעות לשיחה אחת
        [MaxLength(20, ErrorMessage = "השיחה ארוכה מדי. התחילו שיחה חדשה")]
        public List<ChatMessageDTO> Messages { get; set; } = new();
    }
}
