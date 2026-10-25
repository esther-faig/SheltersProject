using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterEligibilityDTO
    {
        //שמייצג את תוצאת השלב הראשון באלגוריתם: האם המקלט רשאי להמשיך לשלב הדירוג.
        public int ShelterId { get; set; }
        public bool IsEligible { get; set; }
        public List<string> Reasons { get; set; }
    }
}
