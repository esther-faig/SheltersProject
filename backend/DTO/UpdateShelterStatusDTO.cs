using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    //החלטת המנהל אם לאשר את המקלט ששותף
    public class UpdateShelterStatusDTO
    {
        public string Status { get; set; } = "";
        public string? RejectionReason { get; set; }
    }
}
