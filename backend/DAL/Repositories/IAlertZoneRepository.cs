using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using NetTopologySuite.Geometries;


namespace DAL.Repositories
{
    public interface IAlertZoneRepository
    {
        //זה נתונים שנכנסים בתחילה ומשתמשים בהם רק לחיפוש

        //AlertZone? GetById(int id);
        //List<AlertZone> GetAll();
        //AlertZone? GetByOfficialCode(string officialCode);
        AlertZone? GetContainingPoint(Point point, DateTime atTime);

        //AlertZone Add(AlertZone entity);
        //bool Delete(int id);
        //AlertZone? Update(AlertZone entity);
    }
}
