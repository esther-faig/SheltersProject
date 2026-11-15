using BLL.Search;
using DAL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GoogleMapsDistanceService : IDistanceService
    {
        private const int BatchSize = 25;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private readonly HttpClient _http;
        private readonly ShelterSearchOptions _options;
        private readonly ILogger<GoogleMapsDistanceService> _logger;

        public GoogleMapsDistanceService(
            HttpClient http, IOptions<ShelterSearchOptions> options, ILogger<GoogleMapsDistanceService> logger)
        {
            _http = http;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<List<RouteResult>> GetRoutesAsync(
            double originLat, double originLon, IReadOnlyList<Shelter> destinations,
            string travelMode, CancellationToken ct)
        {
            // בקשה אחת לכל 25 מקלטים, במקביל
            var tasks = destinations.Chunk(BatchSize)
                .Select(batch => GetBatchSafeAsync(originLat, originLon, batch, travelMode, ct));
            var parts = await Task.WhenAll(tasks);
            return parts.SelectMany(p => p).ToList();
        }

        // אם Google נכשל: מעריכים לפי קו אווירי, כדי שהחיפוש לא ייכשל בזמן אזעקה
        private async Task<List<RouteResult>> GetBatchSafeAsync(
            double lat, double lon, Shelter[] batch, string mode, CancellationToken ct)
        {
            try
            {
                return await GetBatchAsync(lat, lon, batch, mode, ct);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested &&
                (ex is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException))
            {
                _logger.LogWarning(ex, "Google Routes failed, using straight-line estimate");
                return batch.Select(s => Estimate(lat, lon, s, mode)).ToList();
            }
        }

        private async Task<List<RouteResult>> GetBatchAsync(
            double lat, double lon, Shelter[] batch, string mode, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(_options.GoogleApiKey))
                throw new InvalidOperationException("GoogleMaps:ApiKey is missing");

            var body = new
            {
                origins = new[] { Waypoint(lat, lon) },
                destinations = batch.Select(s => Waypoint(s.Latitude, s.Longitude)).ToArray(),
                travelMode = TravelModes.ToGoogle(mode),
                // עומסי תנועה רק לנסיעה
                routingPreference = mode == TravelModes.Driving ? "TRAFFIC_AWARE" : null
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "distanceMatrix/v2:computeRouteMatrix")
            {
                Content = JsonContent.Create(body, options: JsonOptions)
            };
            request.Headers.Add("X-Goog-Api-Key", _options.GoogleApiKey);
            request.Headers.Add("X-Goog-FieldMask",
                "originIndex,destinationIndex,duration,distanceMeters,status,condition");

            using var response = await _http.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

            var results = new List<RouteResult>();
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                // ב-JSON של Google שדה עם ערך 0 לא נשלח בכלל, לכן TryGetProperty ולא GetProperty
                int index = element.TryGetProperty("destinationIndex", out var idx) ? idx.GetInt32() : 0;

                if (!element.TryGetProperty("condition", out var cond) || cond.GetString() != "ROUTE_EXISTS")
                    continue; // אין מסלול למקלט הזה: לא מוסיפים אותו

                double meters = element.TryGetProperty("distanceMeters", out var d) ? d.GetDouble() : 0;
                double seconds = element.TryGetProperty("duration", out var dur)
                    ? double.Parse(dur.GetString()!.TrimEnd('s'), CultureInfo.InvariantCulture)
                    : 0;

                results.Add(new RouteResult(batch[index].ShelterId, meters, (int)Math.Ceiling(seconds), false));
            }
            return results;
        }

        private RouteResult Estimate(double lat, double lon, Shelter s, string mode)
        {
            double meters = GeoMath.HaversineMeters(lat, lon, s.Latitude, s.Longitude) * _options.StraightLineFactor;
            double speed = mode == TravelModes.Driving ? _options.EstimatedDriveSpeedMps : _options.EstimatedWalkSpeedMps;
            return new RouteResult(s.ShelterId, meters, (int)Math.Ceiling(meters / speed), true);
        }

        private static object Waypoint(double lat, double lon) =>
            new { waypoint = new { location = new { latLng = new { latitude = lat, longitude = lon } } } };
    }
}
