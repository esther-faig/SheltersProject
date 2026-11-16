using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IAddressCheckService
    {
        // זורק BusinessRuleException אם הכתובת לא נמצאת או לא תואמת לקואורדינטות
        void EnsureMatches(string? address, string city, double latitude, double longitude);
    }
}
