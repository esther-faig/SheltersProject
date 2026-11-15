using AutoMapper;
using BLL.Search;
using DAL;
using DAL.Repositories;
using DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public class ShelterSearchService : IShelterSearchService
    {
        private readonly IAlertZoneService _alertZoneService;
        private readonly IActiveAlertSource _activeAlerts;
        private readonly IShelterRepository _shelterRepository;
        private readonly IUserPreferenceRepository _userPreferenceRepository;
        private readonly IShelterEligibilityService _eligibility;
        private readonly IShelterRankingService _ranking;
        private readonly IDistanceService _distance;
        private readonly IEmergencyGuidanceService _guidance;
        private readonly IMapper _mapper;
        private readonly ShelterSearchOptions _options;

        public ShelterSearchService(
            IAlertZoneService alertZoneService,
            IActiveAlertSource activeAlerts,
            IShelterRepository shelterRepository,
            IUserPreferenceRepository userPreferenceRepository,
            IShelterEligibilityService eligibility,
            IShelterRankingService ranking,
            IDistanceService distance,
            IEmergencyGuidanceService guidance,
            IMapper mapper,
            IOptions<ShelterSearchOptions> options)
        {
            _alertZoneService = alertZoneService;
            _activeAlerts = activeAlerts;
            _shelterRepository = shelterRepository;
            _userPreferenceRepository = userPreferenceRepository;
            _eligibility = eligibility;
            _ranking = ranking;
            _distance = distance;
            _guidance = guidance;
            _mapper = mapper;
            _options = options.Value;
        }

        public async Task<ShelterSearchResponseDTO> SearchAsync(
            ShelterSearchRequestDTO request, int? userId, CancellationToken ct)
        {
            bool byCar = request.TravelMode == TravelModes.Driving;

            // ── שלב 1: אזור התרעה לפי מיקום המשתמש (Point-in-Polygon) ──
            var zone = _alertZoneService.FindByLocation(request.Latitude, request.Longitude);
            if (zone == null)
            {
                var fallbackType = AlertTypes.Normalize(request.AlertType, _options.DefaultAlertType);
                return NoResults(byCar, "", fallbackType, "Default", 0, false,
                    "לא זוהה אזור התרעה עבור המיקום שלך.");
            }

            // ── שלב 1ב: סוג ההתרעה. התרעה חיה בשרת > מה שהלקוח שלח > ברירת מחדל ──
            var (alertType, alertSource) = await ResolveAlertTypeAsync(zone, request.AlertType, ct);

            // ── שלב 2: זמן התגוננות לפי אזור + סוג התרעה. אם אין, לא ממציאים ──
            int? duration = _alertZoneService.GetDurationSeconds(zone, alertType);
            if (duration == null)
                return NoResults(byCar, zone.AreaName, alertType, alertSource, 0, false,
                    $"אין נתון זמן התגוננות עבור {zone.AreaName}.");

            // ── שלב 3: זמן הגעה מרבי ──
            int maxSeconds = duration.Value - _options.SafetyMarginSeconds;
            if (maxSeconds <= 0)
                return NoResults(byCar, zone.AreaName, alertType, alertSource, 0, false,
                    "זמן ההתגוננות באזור קצר מדי להגעה למקלט.");

            bool offerDriving = !byCar;

            // ── שלב 4: סינון SQL: מאושר + פעיל + ריבוע סביב המשתמש ──
            double speed = byCar ? _options.MaxDriveSpeedMps : _options.MaxWalkSpeedMps;
            double radius = maxSeconds * speed;
            var box = GeoMath.BoundingBox(request.Latitude, request.Longitude, radius);
            var candidates = _shelterRepository.FindCandidates(box.MinLat, box.MaxLat, box.MinLon, box.MaxLon);

            // ── העדפות המשתמש (אורח = אין העדפות) ──
            var preferences = userId.HasValue
                ? _userPreferenceRepository.GetByUserId(userId.Value)
                : new List<UserPreference>();
            var required = preferences.Where(p => p.IsRequired).ToList();
            var optional = preferences.Where(p => !p.IsRequired).ToList();

            // ── שלב 5: עיגול מדויק + העדפות חובה, ממיינים לפי קרבה וחותכים ל-50 ──
            var nearby = candidates
                .Select(s => new
                {
                    Shelter = s,
                    Straight = GeoMath.HaversineMeters(request.Latitude, request.Longitude, s.Latitude, s.Longitude)
                })
                .Where(x => x.Straight <= radius)
                .Where(x => _eligibility.IsEligible(x.Shelter, required))
                .OrderBy(x => x.Straight)
                .Take(_options.MaxCandidatesForRouting)
                .Select(x => x.Shelter)
                .ToList();

            if (nearby.Count == 0)
                return NoResults(byCar, zone.AreaName, alertType, alertSource, maxSeconds, offerDriving,
                    "לא נמצא מקלט מתאים בטווח ההגעה.");

            // ── שלב 6: Google Routes: מרחק וזמן אמיתיים ──
            var routes = await _distance.GetRoutesAsync(
                request.Latitude, request.Longitude, nearby, request.TravelMode, ct);
            var routesById = routes.ToDictionary(r => r.ShelterId);

            // ── שלב 7: פסילת מי שלא מגיעים אליו בזמן (שניות מול שניות) ──
            var routed = nearby
                .Where(s => routesById.ContainsKey(s.ShelterId))
                .Select(s =>
                {
                    var r = routesById[s.ShelterId];
                    return new RoutedShelter(s, r.Meters, r.Seconds, r.IsEstimate);
                })
                .Where(r => r.Seconds <= maxSeconds)
                .ToList();

            if (routed.Count == 0)
                return NoResults(byCar, zone.AreaName, alertType, alertSource, maxSeconds, offerDriving,
                    "לא נמצא מקלט שאפשר להגיע אליו בזמן.");

            // ── שלבים 8-9: ציון + ערימת מקסימום ──
            var ranked = _ranking.Rank(routed, required, optional, maxSeconds, _options.MaxResults);
            // ── שלב 10: בניית התשובה ──
            var results = ranked.Select((r, index) =>
            {
                var dto = _mapper.Map<ShelterSearchResultDTO>(r.Routed.Shelter);
                dto.DistanceMeters = Math.Round(r.Routed.Meters);
                dto.TravelTimeSeconds = r.Routed.Seconds;
                dto.IsEstimate = r.Routed.IsEstimate;
                dto.MatchScore = r.Score;
                dto.MatchReasons = r.Reasons;
                dto.IsRecommended = index == 0;
                return dto;
            }).ToList();

            return new ShelterSearchResponseDTO
            {
                Results = results,
                HasResults = true,
                Message = "נמצא מקלט מומלץ",
                AreaName = zone.AreaName,
                AlertType = alertType,
                AlertSource = alertSource,
                MaxSecondsToReach = maxSeconds,
                OfferDrivingSearch = offerDriving
            };
        }

        // התרעה חיה שחלה על האזור קובעת. אחרת מה שהלקוח ביקש (לבדיקות), ואחרת ברירת המחדל
        private async Task<(string Type, string Source)> ResolveAlertTypeAsync(
            AlertZone zone, string? requested, CancellationToken ct)
        {
            var live = await _activeAlerts.GetCurrentAsync(ct);
            if (live != null && live.Covers(zone.AreaName))
                return (live.AlertType, "Live");

            if (!string.IsNullOrWhiteSpace(requested))
                return (AlertTypes.Normalize(requested, _options.DefaultAlertType), "Requested");

            return (_options.DefaultAlertType, "Default");
        }

        private ShelterSearchResponseDTO NoResults(
            bool byCar, string areaName, string alertType, string alertSource,
            int maxSeconds, bool offerDriving, string message)
        {
            return new ShelterSearchResponseDTO
            {
                HasResults = false,
                Message = message,
                AreaName = areaName,
                AlertType = alertType,
                AlertSource = alertSource,
                MaxSecondsToReach = maxSeconds,
                OfferDrivingSearch = offerDriving,
                EmergencyGuidance = _guidance.Get(byCar)
            };
        }
    }
}