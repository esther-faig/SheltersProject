using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterRankingDTO
    {
        //מציג את תוצאות הדירוג של מקלט מסוים
        //לא משמש לריאקט אלא רק פנימי בשרת
        public int ShelterId { get; set; }
        public double Score { get; set; }
        public int Rank { get; set; }
        public List<string> Reasons { get; set; }
    }
}
