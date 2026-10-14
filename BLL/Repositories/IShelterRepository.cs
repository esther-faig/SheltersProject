using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public interface IShelterRepository
    {
        //משמש להצגת המקלטים בתוצאות החיפוש, כולל מידע על תכונות המקלטים, זמינותם ומיקומם.
        List<ShelterSearchResultDTO> GetAll();
        ShelterSearchResultDTO? GetById(int id);
        ShelterDetailsDTO Add(ShelterRequestDTO dto);
        ShelterDetailsDTO? Update(int id, ShelterRequestDTO dto);
        bool Delete(int id);
    }
}
