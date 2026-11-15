using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Search
{
    public interface IShelterEligibilityService
    {
        // האם המקלט עומד בתנאי הסף: מאושר, פעיל, וכל העדפות החובה
        bool IsEligible(Shelter shelter, IReadOnlyCollection<UserPreference> requiredPreferences);
    }
}
