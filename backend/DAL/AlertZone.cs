using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace DAL
{
    public class AlertZone
    {
        public int AlertZoneId { get; set; }

        public string AreaName { get; set; }


        // הקוד הרשמי של האזור
        public string OfficialCode { get; set; }



        // הגבול הגיאוגרפי של אזור ההתרעה (Polygon / MultiPolygon)
        public Geometry AreaGeometry { get; set; }

        // זמני ההתגוננות ששיכים בתוך אזור ההתרעה
        public ICollection<ProtectionTime> ProtectionTimes { get; set; } = new List<ProtectionTime>();

        //המקלטים שנמצאים בתוך אזור ההתרעה
        public List<Shelter> Shelters { get; set; } = new List<Shelter>();
    }
}

