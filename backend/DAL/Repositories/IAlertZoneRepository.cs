using DAL;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IAlertZoneRepository
    {

        // האזור שמכיל את הנקודה, יחד עם זמני ההתגוננות שתקפים ברגע atTime
        AlertZone? GetContainingPoint(Point point, DateTime atTime);
    }
}
