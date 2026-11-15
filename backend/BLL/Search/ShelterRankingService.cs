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
            IReadOnlyList<UserPreference> requiredPreferences,
            IReadOnlyList<UserPreference> optionalPreferences,
            int maxSeconds, int take)
        {
            // Priority 1 = הכי חשוב. הרגילות מסודרות, והראשונה מקבלת את המשקל הגבוה ביותר (n..1)
            var orderedOptional = optionalPreferences
                .OrderBy(p => p.Priority).ThenBy(p => p.PreferenceId).ToList();
            int n = orderedOptional.Count;

            // חובה מקבלת משקל גבוה מכל העדפה רגילה, כדי שמקלט שאישר דרישת חובה ינצח מקלט שלא ידוע
            int requiredWeight = n + 1;

            var weighted = new List<(UserPreference Pref, int Weight, bool Required)>();
            weighted.AddRange(requiredPreferences.OrderBy(p => p.PreferenceId)
                .Select(p => (p, requiredWeight, true)));
            weighted.AddRange(orderedOptional.Select((p, i) => (p, n - i, false)));
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
                double earned = 0;
                var reasons = new List<string> { TravelReason(routed) };

                foreach (var (pref, weight, isRequired) in weighted)
                {
                    string name = pref.Preference?.Name ?? $"העדפה {pref.PreferenceId}";

                    switch (ShelterFeatureLookup.StateOf(routed.Shelter, pref.PreferenceId))
                    {
                        case FeatureState.Yes:
                            earned += weight;
                            reasons.Add(isRequired
                                ? $"עומד בדרישת החובה שלך: {name}"
                                : $"מתאים להעדפה שלך: {name}");
                            break;

                        case FeatureState.No:
                            // העדפת חובה עם "אין" כבר נפסלה בסינון, לכן כאן זה רק העדפה רגילה
                            reasons.Add($"אין במקלט: {name}");
                            break;

                        default: // Unknown: 0 נקודות, ותווית ברורה
                            reasons.Add(isRequired
                                ? $"לא ידוע אם יש: {name} (דרישת חובה, לא אומת)"
                                : $"לא ידוע אם יש: {name}");
                            break;
                    }
                }

                // רכיבים מנורמלים ל-0..100. 100 = הגעה מיידית, 0 = בדיוק בגבול הזמן
                double timeScore = Math.Clamp(100.0 * (1.0 - (double)routed.Seconds / maxSeconds), 0, 100);
                double prefScore = totalWeight > 0 ? 100.0 * earned / totalWeight : 0;

                // אורח או משתמש בלי שום העדפה: רק ציון הזמן (אין מסלול קוד נפרד)
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