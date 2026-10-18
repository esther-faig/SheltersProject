using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IUserService
    {
        public List<UserProfileDTO> GetAll();
        public UserProfileDTO? GetById(int id);



    }
}
