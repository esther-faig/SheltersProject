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


        //בשביל חיפוש המקלטים לפי תנאים מסוימים
        //למנהל- עבור מסך הניהול שהוא רוצה לראות את כל המקלטים
        //נשלח לפונקציה פעמיים NULL למנהל-
        List<Shelter> Find(ShelterStatus? status, bool? isActive);
        
        //למשתמש-להצגת המקלטים שהוא שיתף
        List<Shelter> GetByUserId(int userId);
        Shelter? GetById(int id);
        Shelter Add(Shelter entity);
        Shelter? Update(Shelter entity);
        bool Delete(int id);
        Shelter? GetOwned(int id, int userId);   // מקלט לפי מזהה *וגם* של המשתמש הזה
    }
}
