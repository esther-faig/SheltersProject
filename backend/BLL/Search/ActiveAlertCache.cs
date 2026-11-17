using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public record AlertCacheStatus(
        ActiveAlert? CurrentAlert,
        DateTime? LastSuccessfulPollUtc,
        DateTime? LastAlertSeenUtc,
        IReadOnlyList<AlertSourceStatus> Sources);

    public interface IActiveAlertCache
    {
        // ההתרעה החיה כרגע (null = אין, או שהמידע ישן מדי)
        ActiveAlert? Current { get; }
        AlertCacheStatus GetStatus();
    }

    public class ActiveAlertCache : IActiveAlertCache
    {
        private sealed record State(
            ActiveAlert? Alert, DateTime? LastSeen, DateTime? LastSuccess,
            IReadOnlyList<AlertSourceStatus> Sources);

        private readonly AlertServerOptions _options;
        private volatile State _state = new(null, null, null, Array.Empty<AlertSourceStatus>());

        public ActiveAlertCache(IOptions<AlertServerOptions> options) => _options = options.Value;

        // נקרא רק משירות הרקע (כותב יחיד)
        public void Update(AlertPollResult result, DateTime nowUtc)
        {
            var s = _state;
            var alert = s.Alert;
            var lastSeen = s.LastSeen;
            var lastSuccess = s.LastSuccess;

            if (result.Succeeded)
            {
                lastSuccess = nowUtc;

                if (result.Alert != null)
                {
                    alert = result.Alert;
                    lastSeen = nowUtc;
                }
                else if (alert != null && lastSeen.HasValue &&
                         (nowUtc - lastSeen.Value).TotalSeconds < _options.RetentionSeconds)
                {
                    // ההתרעה נעלמה מהשרת, אבל עדיין בתוך חלון ההחזקה
                }
                else
                {
                    alert = null;
                }
            }
            // קריאה שנכשלה לא מוחקת כלום: ההתרעה האחרונה נשארת עד שתתיישן

            _state = new State(alert, lastSeen, lastSuccess, result.Sources);
        }

        public ActiveAlert? Current
        {
            get
            {
                var s = _state;
                if (s.Alert == null || s.LastSeen == null) return null;

                double age = (DateTime.UtcNow - s.LastSeen.Value).TotalSeconds;
                double limit = Math.Max(_options.RetentionSeconds, _options.StaleAfterSeconds);
                return age <= limit ? s.Alert : null;
            }
        }

        public AlertCacheStatus GetStatus()
        {
            var s = _state;
            return new AlertCacheStatus(Current, s.LastSuccess, s.LastSeen, s.Sources);
        }
    }
}