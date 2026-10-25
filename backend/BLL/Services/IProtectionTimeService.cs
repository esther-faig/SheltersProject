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
        // לייבוא / עדכון מהמקור הרשמי. שומר היסטוריה, ואם הערך לא השתנה לא עושה כלום

        ProtectionTime SetDuration(int alertZoneId, string alertType, int durationSeconds, string source);

    }
}
