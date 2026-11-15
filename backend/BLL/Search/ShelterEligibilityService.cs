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
        private readonly ShelterSearchOptions _options;

        public ShelterEligibilityService(IOptions<ShelterSearchOptions> options)
        {
            _options = options.Value;
        }

        public bool IsEligible(Shelter shelter, IReadOnlyCollection<UserPreference> requiredPreferences)
        {
            // הגנה כפולה (ה-SQL כבר סינן): מקלט לא פעיל / לא מאושר לא מוצג בכלל
            if (!shelter.IsActive || shelter.Status != ShelterStatus.Approved)
                return false;

            if (requiredPreferences.Count == 0)
                return true;

            var features = shelter.ShelterFeatures.ToDictionary(f => f.PreferenceId, f => f.IsAvailable);

            foreach (var required in requiredPreferences)
            {
                features.TryGetValue(required.PreferenceId, out var available); // אין רשומה = null = לא ידוע

                if (available == false) return false;                            // בוודאות אין: נפסל תמיד
                if (available == null && _options.UnknownFailsRequired) return false; // לא ידוע: לפי הקונפיגורציה
            }
            return true;
        }
    }
}
