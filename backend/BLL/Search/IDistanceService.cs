using BLL.Services;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Search
{
    // מבודד את ספק המפות: אפשר להחליף ספק בלי לגעת באלגוריתם
    public interface IDistanceService
    {
        // מחזיר תוצאה לכל מקלט שיש אליו מסלול (מקלט בלי מסלול לא מופיע ברשימה)
        Task<List<RouteResult>> GetRoutesAsync(
            double originLat, double originLon, IReadOnlyList<Shelter> destinations,
            string travelMode, CancellationToken ct);
    }
}
