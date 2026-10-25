using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IProtectionTimeRepository
    {
        //משמש רק לחיפוש

        //ProtectionTime? GetById(int id);
        //List<ProtectionTime> GetAll();
        //// כל ההיסטוריה של אזור, מהחדש לישן
        //List<ProtectionTime> GetByAlertZoneId(int alertZoneId);


        ProtectionTime Add(ProtectionTime entity);

        // הרשומה שתקפה ברגע atTime לאזור ולסוג התרעה
        ProtectionTime? GetValidAt(int alertZoneId, string alertType, DateTime atTime);
        //bool Delete(int id);
        //ProtectionTime? Update(ProtectionTime entity);
    }
}
