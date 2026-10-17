using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserPreferenceRepository
    {
        List<UserPreference> GetAll();
        UserPreference? GetById(int id);
        List<UserPreference> GetByUserId(int userId);// או השאר GetById אבל עם List


        UserPreference Add(UserPreference entity);
        UserPreference? Update(int id, UserPreference entity);

        bool Delete(int id);
    }
}
