using System.Text.Json;

namespace BLL.Services
{
    // התרעה חיה אחת: הסוג (Alarm / Warning) והאזורים שהיא חלה עליהם
    public record ActiveAlert(string AlertType, IReadOnlyList<string> Areas)
    {
        // האם ההתרעה חלה על האזור הזה (השוואה לפי שם, בלי תלות בגרשיים ומקפים שונים)
        public bool Covers(string areaName) =>
            Areas.Any(a => string.Equals(Normalize(a), Normalize(areaName), StringComparison.OrdinalIgnoreCase));

        private static string Normalize(string s) =>
            string.Join(' ', s.Replace('–', '-').Replace('־', '-')
                              .Replace('״', '"').Replace('”', '"')
                              .Replace('׳', '\'').Replace('’', '\'')
                              .Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    // מצב החיבור לשרת התרעות אחד
    public record AlertSourceStatus(string Source, bool Succeeded, string? Error, DateTime CheckedAtUtc);

    // Succeeded = לפחות שרת אחד ענה בהצלחה (גם אם אין התרעה). Alert = null כשאין התרעה
    public record AlertPollResult(bool Succeeded, ActiveAlert? Alert, IReadOnlyList<AlertSourceStatus> Sources);

    public interface IActiveAlertSource
    {
        // לעולם לא זורק חריגה (חוץ מביטול)
        Task<AlertPollResult> PollAsync(CancellationToken ct);
    }

    // קורא מ-alerts.json אחד (בפורמט של פיקוד העורף)
    public class OrefActiveAlertSource : IActiveAlertSource
    {
        private readonly HttpClient _http;

        public OrefActiveAlertSource(HttpClient http) => _http = http;

        private string Name => _http.BaseAddress?.ToString() ?? "unknown";

        public async Task<AlertPollResult> PollAsync(CancellationToken ct)
        {
            try
            {
                using var response = await _http.GetAsync("WarningMessages/alert/alerts.json", ct);
                response.EnsureSuccessStatusCode();

                // כשאין התרעה הגוף ריק (BOM בלבד), וזה תקין
                var text = (await response.Content.ReadAsStringAsync(ct)).Trim('\uFEFF', '\0', ' ', '\r', '\n', '\t');
                var alert = text.Length == 0 ? null : Parse(text);
                return Result(true, alert, null);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested &&
                (ex is HttpRequestException or TaskCanceledException or JsonException
                    or KeyNotFoundException or InvalidOperationException))
            {
                // Timeout מגיע כ-TaskCanceledException בלי שהטוקן שלנו בוטל
                return Result(false, null, ex.Message);
            }
        }

        private static ActiveAlert? Parse(string text)
        {
            using var doc = JsonDocument.Parse(text);
            var root = doc.RootElement;

            // cat מגיע כמחרוזת ("1"), אבל מטפלים גם במספר
            var catElement = root.GetProperty("cat");
            string cat = catElement.ValueKind == JsonValueKind.String
                ? catElement.GetString() ?? ""
                : catElement.GetRawText();

            string? type = cat switch
            {
                "1" => AlertTypes.Alarm,      // ירי רקטות וטילים
                "14" => AlertTypes.Warning,   // התראה מקדימה
                _ => null                     // סוגי התרעה אחרים לא נתמכים כרגע
            };
            if (type == null) return null;

            var areas = root.GetProperty("data").EnumerateArray()
                .Select(e => e.GetString() ?? "")
                .Where(s => s.Length > 0)
                .ToList();

            return new ActiveAlert(type, areas);
        }

        private AlertPollResult Result(bool ok, ActiveAlert? alert, string? error) =>
            new(ok, alert, new[] { new AlertSourceStatus(Name, ok, error, DateTime.UtcNow) });
    }

    // כמה שרתים במקביל (מדומה + אמיתי). אזעקה קודמת להתראה מקדימה
    public class CompositeActiveAlertSource : IActiveAlertSource
    {
        private readonly IReadOnlyList<IActiveAlertSource> _sources;

        public CompositeActiveAlertSource(IEnumerable<IActiveAlertSource> sources)
            => _sources = sources.ToList();

        public async Task<AlertPollResult> PollAsync(CancellationToken ct)
        {
            var results = await Task.WhenAll(_sources.Select(s => s.PollAsync(ct)));

            var statuses = results.SelectMany(r => r.Sources).ToList();
            bool ok = results.Any(r => r.Succeeded);

            var alerts = results.Where(r => r.Alert != null).Select(r => r.Alert!).ToList();
            ActiveAlert? merged = null;
            if (alerts.Count > 0)
            {
                var type = alerts.Any(a => a.AlertType == AlertTypes.Alarm)
                    ? AlertTypes.Alarm : AlertTypes.Warning;
                var areas = alerts.Where(a => a.AlertType == type)
                                  .SelectMany(a => a.Areas).Distinct().ToList();
                merged = new ActiveAlert(type, areas);
            }

            return new AlertPollResult(ok, merged, statuses);
        }
    }
}