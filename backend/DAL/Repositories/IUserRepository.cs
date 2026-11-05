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
        User? GetByEmail(string email);
        //לצורך פונקצית ההרשמה
        User Add(User entity);
        bool EmailExists(string email);
        User? Update(User entity);
        //אין אפשרות למחוק משתמשים, לכן אין פונקצית מחיקה
        //bool Delete(int id);
    }
}
