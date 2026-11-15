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

    public class ShelterRankingService : IShelterRankingService
    {
        private readonly ShelterSearchOptions _options;

        public ShelterRankingService(IOptions<ShelterSearchOptions> options)
        {
            _options = options.Value;
        }

        public List<RankedShelter> Rank(
            IReadOnlyList<RoutedShelter> shelters,
            IReadOnlyList<UserPreference> optionalPreferences,
            int maxSeconds, int take)
        {
            // Priority 1 = הכי חשוב, לכן משקל = (הגדול ביותר - Priority + 1)
            int maxPriority = optionalPreferences.Count == 0 ? 0 : optionalPreferences.Max(p => p.Priority);
            var weighted = optionalPreferences
                .Select(p => (Pref: p, Weight: maxPriority - p.Priority + 1))
                .OrderByDescending(x => x.Weight)
                .ToList();
            double totalWeight = weighted.Sum(x => x.Weight);

            // נרמול המשקלים כך שיסתכמו ל-1
            double wSum = _options.PreferenceWeight + _options.TravelTimeWeight;
            double wPref = _options.PreferenceWeight / wSum;
            double wTime = _options.TravelTimeWeight / wSum;

            // PriorityQueue של .NET היא ערימת מינימום. מכניסים ציון שלילי, ולכן היא מתנהגת כערימת מקסימום.
            // שוברי שוויון (בסדר הזה): ציון גבוה, זמן הגעה קצר, מזהה מקלט נמוך
            var heap = new PriorityQueue<RankedShelter, (double NegScore, int Seconds, int Id)>();

            foreach (var routed in shelters)
            {
                var features = routed.Shelter.ShelterFeatures.ToDictionary(f => f.PreferenceId, f => f.IsAvailable);

                double earned = 0;
                var reasons = new List<string> { TravelReason(routed) };

                foreach (var (pref, weight) in weighted)
                {
                    string name = pref.Preference?.Name ?? $"העדפה {pref.PreferenceId}";
                    features.TryGetValue(pref.PreferenceId, out var available);

                    if (available == true) { earned += weight; reasons.Add($"מתאים להעדפה שלך: {name}"); }
                    else if (available == false) reasons.Add($"אין במקלט: {name}");
                    else reasons.Add($"לא ידוע אם יש: {name}");
                }

                // רכיבים מנורמלים ל-0..100. 100 = הגעה מיידית, 0 = בדיוק בגבול הזמן
                double timeScore = Math.Clamp(100.0 * (1.0 - (double)routed.Seconds / maxSeconds), 0, 100);
                double prefScore = totalWeight > 0 ? 100.0 * earned / totalWeight : 0;

                // אורח או משתמש בלי העדפות רגילות: רק ציון הזמן (אין מסלול קוד נפרד)
                double score = totalWeight > 0 ? wPref * prefScore + wTime * timeScore : timeScore;

                heap.Enqueue(
                    new RankedShelter(routed, Math.Round(score, 1), reasons),
                    (-score, routed.Seconds, routed.Shelter.ShelterId));
            }

            var result = new List<RankedShelter>();
            while (result.Count < take && heap.TryDequeue(out var best, out _))
                result.Add(best);
            return result;
        }

        private static string TravelReason(RoutedShelter r)
        {
            string time = r.Seconds < 60 ? $"{r.Seconds} שניות" : $"כ-{Math.Ceiling(r.Seconds / 60.0)} דקות";
            string estimate = r.IsEstimate ? " (הערכה)" : "";
            return $"זמן הגעה: {time}{estimate}, {Math.Round(r.Meters)} מטר";
        }
    }
}