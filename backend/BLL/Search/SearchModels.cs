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
}
