using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IAlertZoneService
    {
        //אין DTO

        AlertZone? GetById(int id);
        List<AlertZone> GetAll();
        AlertZone Add(AlertZone entity);
        bool Delete(int id);
        AlertZone? Update(int id, AlertZone entity);
    }
}
