using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public static class AlertTypes
    {
        public const string Alarm = "Alarm";     // אזעקה
        public const string Warning = "Warning"; // התראה מוקדמת

        public static string Normalize(string? requested, string fallback)
        {
            var value = string.IsNullOrWhiteSpace(requested) ? fallback : requested;
            return string.Equals(value, Warning, StringComparison.OrdinalIgnoreCase) ? Warning : Alarm;
        }
    }

    public static class TravelModes
    {
        public const string Walking = "WALKING";
        public const string Driving = "DRIVING";

        public static string ToGoogle(string mode) => mode == Driving ? "DRIVE" : "WALK";
    }
}
