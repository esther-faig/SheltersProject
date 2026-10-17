using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User? GetById(int id);
        //לצורך פונקצית ההרשמה
        User Add(User entity);

        User? Update(int id, User entity);
       

        //אין אפשרות למחוק משתמשים, לכן אין פונקצית מחיקה
        //bool Delete(int id);
    }
}
