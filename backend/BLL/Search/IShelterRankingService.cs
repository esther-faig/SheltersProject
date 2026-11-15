using BLL.Services;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Search
{
    public interface IShelterRankingService
    {
        List<RankedShelter> Rank(
            IReadOnlyList<RoutedShelter> shelters,
            IReadOnlyList<UserPreference> optionalPreferences,
            int maxSeconds, int take);
    }
}
