using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;

namespace BLL.Services
{
    // תוצאת מסלול למקלט אחד (מ-Google, או הערכה)
    public record RouteResult(int ShelterId, double Meters, int Seconds, bool IsEstimate);

    // מקלט + המסלול אליו
    public record RoutedShelter(Shelter Shelter, double Meters, int Seconds, bool IsEstimate);

    // מקלט אחרי דירוג
    public record RankedShelter(RoutedShelter Routed, double Score, List<string> Reasons);

    // true = Yes (התאמה), false = No (התנגשות), null או שורה חסרה = Unknown (לא ידוע)
    public enum FeatureState { Unknown, Yes, No }

    public static class ShelterFeatureLookup
    {
        public static FeatureState StateOf(Shelter shelter, int preferenceId)
        {
            var feature = shelter.ShelterFeatures.FirstOrDefault(f => f.PreferenceId == preferenceId);
            return feature?.IsAvailable switch
            {
                true => FeatureState.Yes,
                false => FeatureState.No,
                _ => FeatureState.Unknown   // null, או אין שורה בכלל
            };
        }
    }
}
