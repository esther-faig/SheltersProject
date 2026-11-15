using DAL;
using BLL.Search;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{

    public class ShelterEligibilityService : IShelterEligibilityService
    {
        public bool IsEligible(Shelter shelter, IReadOnlyCollection<UserPreference> requiredPreferences)
        {
            // הגנה כפולה (ה-SQL כבר סינן): מקלט לא פעיל / לא מאושר לא מוצג בכלל
            if (!shelter.IsActive || shelter.Status != ShelterStatus.Approved)
                return false;

            foreach (var required in requiredPreferences)
            {
                // רק התנגשות ודאית (false) פוסלת. "לא ידוע" עובר, והציון יבדיל בינו לבין מקלט מאומת
                if (ShelterFeatureLookup.StateOf(shelter, required.PreferenceId) == FeatureState.No)
                    return false;
            }
            return true;
        }
    }
}