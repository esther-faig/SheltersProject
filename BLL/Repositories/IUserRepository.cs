using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public interface IUserRepository
    {
        List<UserProfileDTO> GetAll();
        UserProfileDTO? GetById(int id);
        UserProfileDTO Add(UserProfileDTO dto);
        UserProfileDTO? Update(int id, UserProfileDTO dto);
        bool Delete(int id);
    }
}
