using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

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

    public interface IActiveAlertSource
    {
        // null = אין התרעה פעילה, או שהשרת לא זמין. לעולם לא זורק חריגה
        Task<ActiveAlert?> GetCurrentAsync(CancellationToken ct);
    }

    // קורא מ-alerts.json (בפורמט של פיקוד העורף). מצביעים אותו לשרת המדומה או לאמיתי דרך AlertServer:BaseUrl
    public class OrefActiveAlertSource : IActiveAlertSource
    {
        private readonly HttpClient _http;
        private readonly ILogger<OrefActiveAlertSource> _logger;

        public OrefActiveAlertSource(HttpClient http, ILogger<OrefActiveAlertSource> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<ActiveAlert?> GetCurrentAsync(CancellationToken ct)
        {
            try
            {
                using var response = await _http.GetAsync("WarningMessages/alert/alerts.json", ct);
                response.EnsureSuccessStatusCode();

                // כשאין התרעה הגוף ריק (BOM בלבד), וזה תקין
                var text = (await response.Content.ReadAsStringAsync(ct)).Trim('\uFEFF', '\0', ' ', '\r', '\n', '\t');
                if (text.Length == 0) return null;

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
            catch (Exception ex) when (!ct.IsCancellationRequested &&
                (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException or InvalidOperationException))
            {
                // בזמן חירום עדיף להמשיך בלי המידע הזה מאשר להיכשל
                _logger.LogWarning(ex, "Could not read current alert, continuing without it");
                return null;
            }
        }
    }
}