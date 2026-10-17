using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IShelterRepository
    {
        //ShelterSearchResultDTO
        //משמש להצגת המקלטים בתוצאות החיפוש, כולל מידע על תכונות המקלטים, זמינותם ומיקומם.
        List<Shelter> GetAll();
        Shelter? GetById(int id);
        Shelter Add(Shelter dtentityo);
        Shelter? Update(int id, Shelter entity);
        bool Delete(int id);
    }
}
