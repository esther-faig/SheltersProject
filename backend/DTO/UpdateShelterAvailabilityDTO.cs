using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UpdateShelterAvailabilityDTO
    {
        //בעל המקלט יכול לבטל לזמן מה את המקלט ששיתף
        public bool IsActive { get; set; }
    }
}
