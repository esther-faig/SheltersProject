using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public class AlertPollingService : BackgroundService
    {
        private readonly IActiveAlertSource _source;
        private readonly ActiveAlertCache _cache;
        private readonly AlertServerOptions _options;
        private readonly ILogger<AlertPollingService> _logger;

        private readonly Dictionary<string, bool> _lastOk = new();
        private string? _lastAlertKey;

        public AlertPollingService(
            IActiveAlertSource source, ActiveAlertCache cache,
            IOptions<AlertServerOptions> options, ILogger<AlertPollingService> logger)
        {
            _source = source;
            _cache = cache;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stop)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Math.Max(1, _options.PollIntervalSeconds)));
            try
            {
                do
                {
                    await PollOnce(stop);
                }
                while (await timer.WaitForNextTickAsync(stop));
            }
            catch (OperationCanceledException) when (stop.IsCancellationRequested)
            {
                // כיבוי רגיל של האפליקציה
            }
        }

        private async Task PollOnce(CancellationToken stop)
        {
            AlertPollResult result;
            try
            {
                result = await _source.PollAsync(stop);
            }
            catch (OperationCanceledException) when (stop.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                // השירות לעולם לא נופל: בזמן חירום ממשיכים לנסות
                _logger.LogError(ex, "Alert poll crashed");
                result = new AlertPollResult(false, null, Array.Empty<AlertSourceStatus>());
            }

            LogChanges(result);
            _cache.Update(result, DateTime.UtcNow);
        }

        // רושמים ללוג רק שינויים, כדי לא להציף את הלוג כל 2 שניות
        private void LogChanges(AlertPollResult result)
        {
            foreach (var s in result.Sources)
            {
                if (_lastOk.TryGetValue(s.Source, out var was) && was == s.Succeeded) continue;
                _lastOk[s.Source] = s.Succeeded;

                if (s.Succeeded) _logger.LogInformation("Alert source OK: {Source}", s.Source);
                else _logger.LogWarning("Alert source FAILED: {Source}. {Error}", s.Source, s.Error);
            }

            var key = result.Alert == null
                ? null
                : $"{result.Alert.AlertType}:{string.Join(',', result.Alert.Areas)}";
            if (key != _lastAlertKey)
            {
                _lastAlertKey = key;
                if (result.Alert == null) _logger.LogInformation("No active alert");
                else _logger.LogInformation("Active alert: {Type}, {Count} areas",
                    result.Alert.AlertType, result.Alert.Areas.Count);
            }
        }
    }
}
