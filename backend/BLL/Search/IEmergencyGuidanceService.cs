using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Search
{
    public interface IEmergencyGuidanceService
    {
        EmergencyGuidanceDTO Get(bool inVehicle);
    }
}
