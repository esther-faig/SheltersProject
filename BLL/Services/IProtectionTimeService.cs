using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IProtectionTimeService
    {
        ProtectionTime? GetById(int id);
        List<ProtectionTime> GetAll();
        ProtectionTime Add(ProtectionTime entity);
        bool Delete(int id);
        ProtectionTime? Update(int id, ProtectionTime entity);
    }
}
