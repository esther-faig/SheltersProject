using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;


namespace BLL.Repositories
{
    public interface IAlertZoneRepository
    {
        AlertZone? GetById(int id);
        List<AlertZone> GetAll();
        AlertZone Add(AlertZone entity);
        bool Delete(int id);
        AlertZone? Update( int id,AlertZone entity);
    }
}
