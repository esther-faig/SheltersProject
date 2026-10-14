using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public interface IUserPreferenceRepository
    {
        List<UserPreferenceDTO> GetAll();
        UserPreferenceDTO? GetById(int id);
        List<UserPreferenceDTO> GetByUserId(int userId);// או השאר GetById אבל עם List
        void SaveUserPreferences(int userId, SaveUserPreferencesRequestDTO dto);

        //אין צורך בפונקציות האלו כי ההעדפות נשמרות בטבלת קישור בין משתמש להעדפה, ולכן אין צורך להוסיף, לעדכן או למחוק העדפות ספציפיות. כל מה שצריך זה לשלוף את ההעדפות של המשתמש לפי מזהה המשתמש.
        //ShelterSearchResultDTO Add(ShelterSearchResultDTO dto);
        //UserPreferenceDTO? Update(int id, ShelterSearchResultDTO dto);
        //bool Delete(int id);
    }
}
