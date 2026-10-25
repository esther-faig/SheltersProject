using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserPreference
    {
        public int UserPreferenceId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PreferenceId { get; set; }
        public Preference Preference { get; set; }

        public int Priority { get; set; }
        public bool IsRequired { get; set; }
    }
}
