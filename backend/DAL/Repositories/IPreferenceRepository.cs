using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IPreferenceRepository
    {
        List<Preference> GetAll();
        Preference? GetById(int id);
        Preference Add(Preference entity);
        Preference? Update(int id, Preference entity);
        bool Delete(int id);
        bool IsInUse(int id);   // האם ההעדפה בשימוש אצל משתמש או במקלט

    }
}
