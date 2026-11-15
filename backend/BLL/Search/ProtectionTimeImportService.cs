using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http.Json;
using System.Text.Json.Serialization;
using DAL.Repositories;

namespace BLL.Services
{
    // ⚠ הנחה לגבי הפורמט של שרת הפייתון: GET {BaseUrl}/protection-times
    // מחזיר מערך כזה. אם הפורמט אצלך שונה, משנים רק את המחלקה הזו
    public class ProtectionTimeItem
    {
        [JsonPropertyName("officialCode")] public string OfficialCode { get; set; } = "";
        [JsonPropertyName("areaName")] public string AreaName { get; set; } = "";
        [JsonPropertyName("alarmSeconds")] public int AlarmSeconds { get; set; }      // אזעקה
        [JsonPropertyName("warningSeconds")] public int WarningSeconds { get; set; }  // התראה מוקדמת
    }

    public interface IProtectionTimeSource
    {
        Task<List<ProtectionTimeItem>> GetAllAsync(CancellationToken ct);
    }

    // הלקוח של שרת הפייתון המדומה (בהמשך אפשר להחליף בלקוח אמיתי של פיקוד העורף)
    public class MockProtectionTimeSource : IProtectionTimeSource
    {
        private readonly HttpClient _http;
        public MockProtectionTimeSource(HttpClient http) => _http = http;

        public async Task<List<ProtectionTimeItem>> GetAllAsync(CancellationToken ct)
            => await _http.GetFromJsonAsync<List<ProtectionTimeItem>>("protection-times", ct) ?? new();
    }

    public record ImportResult(int Updated, List<string> NotFound);

    public interface IProtectionTimeImportService
    {
        Task<ImportResult> ImportAsync(CancellationToken ct);
    }

    public class ProtectionTimeImportService : IProtectionTimeImportService
    {
        private readonly IProtectionTimeSource _source;
        private readonly IAlertZoneRepository _zones;
        private readonly IProtectionTimeService _times;

        public ProtectionTimeImportService(
            IProtectionTimeSource source, IAlertZoneRepository zones, IProtectionTimeService times)
        {
            _source = source;
            _zones = zones;
            _times = times;
        }

        public async Task<ImportResult> ImportAsync(CancellationToken ct)
        {
            var items = await _source.GetAllAsync(ct);
            int updated = 0;
            var notFound = new List<string>();

            foreach (var item in items)
            {
                var zone = _zones.GetByOfficialCode(item.OfficialCode);
                if (zone == null) { notFound.Add(item.AreaName); continue; }

                // SetDuration שומר היסטוריה, ואם הערך לא השתנה לא עושה כלום
                _times.SetDuration(zone.AlertZoneId, AlertTypes.Alarm, item.AlarmSeconds, "MockServer");
                _times.SetDuration(zone.AlertZoneId, AlertTypes.Warning, item.WarningSeconds, "MockServer");
                updated++;
            }
            return new ImportResult(updated, notFound);
        }
    }
}
