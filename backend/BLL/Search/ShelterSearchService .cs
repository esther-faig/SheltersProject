
using AutoMapper;
using BLL.Search;
using DAL;
using DAL.Repositories;
using DTO;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public class ShelterSearchService : IShelterSearchService
    {
        private readonly IAlertZoneService _alertZoneService;
        private readonly IActiveAlertCache _alertCache;
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
            IActiveAlertCache alertCache,
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
            _alertCache = alertCache;
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
            ShelterSearchRequestDTO request,
            int? userId,
            CancellationToken ct)
        {
            bool byCar = request.TravelMode == TravelModes.Driving;

            // שלב 1: זיהוי אזור ההתרעה לפי מיקום המשתמש
            var zone = _alertZoneService.FindByLocation(
                request.Latitude,
                request.Longitude);

            if (zone == null)
            {
                return NoResults(
                    byCar,
                    "",
                    _options.DefaultAlertType,
                    "Default",
                    0,
                    false,
                    "לא זוהה אזור התרעה עבור המיקום שלך.");
            }

            // שלב 2: קביעת סוג ההתרעה לפי שרת הדמה או ברירת המחדל
            var (alertType, alertSource) = ResolveAlertType(zone);

            // שלב 3: קבלת זמן ההתגוננות לפי האזור וסוג ההתרעה
            int? duration = _alertZoneService.GetDurationSeconds(
                zone,
                alertType);

            if (duration == null)
            {
                return NoResults(
                    byCar,
                    zone.AreaName,
                    alertType,
                    alertSource,
                    0,
                    false,
                    $"אין נתון זמן התגוננות עבור {zone.AreaName}.");
            }

            // שלב 4: חישוב זמן ההגעה המרבי
            int maxSeconds = duration.Value - _options.SafetyMarginSeconds;

            if (maxSeconds <= 0)
            {
                return NoResults(
                    byCar,
                    zone.AreaName,
                    alertType,
                    alertSource,
                    0,
                    false,
                    "זמן ההתגוננות באזור קצר מדי להגעה למקלט.");
            }

            bool offerDriving = !byCar;

            // שלב 5: סינון ראשוני של מקלטים לפי אזור גיאוגרפי
            double speed = byCar
                ? _options.MaxDriveSpeedMps
                : _options.MaxWalkSpeedMps;

            double radius = maxSeconds * speed;

            var box = GeoMath.BoundingBox(
                request.Latitude,
                request.Longitude,
                radius);

            var candidates = _shelterRepository.FindCandidates(
                box.MinLat,
                box.MaxLat,
                box.MinLon,
                box.MaxLon);

            // שלב 6: טעינת העדפות המשתמש
            // אורח שלא מחובר למערכת מקבל חיפוש ללא העדפות אישיות
            var preferences = userId.HasValue
                ? _userPreferenceRepository.GetByUserId(userId.Value)
                : new List<UserPreference>();

            var required = preferences
                .Where(p => p.IsRequired)
                .ToList();

            var optional = preferences
                .Where(p => !p.IsRequired)
                .ToList();

            // שלב 7: סינון לפי מרחק והעדפות חובה
            var nearby = candidates
                .Select(s => new
                {
                    Shelter = s,
                    Straight = GeoMath.HaversineMeters(
                        request.Latitude,
                        request.Longitude,
                        s.Latitude,
                        s.Longitude)
                })
                .Where(x => x.Straight <= radius)
                .Where(x => _eligibility.IsEligible(
                    x.Shelter,
                    required))
                .OrderBy(x => x.Straight)
                .Take(_options.MaxCandidatesForRouting)
                .Select(x => x.Shelter)
                .ToList();

            if (nearby.Count == 0)
            {
                return NoResults(
                    byCar,
                    zone.AreaName,
                    alertType,
                    alertSource,
                    maxSeconds,
                    offerDriving,
                    "לא נמצא מקלט מתאים בטווח ההגעה.");
            }

            // שלב 8: חישוב מסלולים וזמני הגעה
            var routes = await _distance.GetRoutesAsync(
                request.Latitude,
                request.Longitude,
                nearby,
                request.TravelMode,
                ct);

            var routesById = routes.ToDictionary(r => r.ShelterId);

            // שלב 9: פסילת מקלטים שלא ניתן להגיע אליהם בזמן
            var routed = nearby
                .Where(s => routesById.ContainsKey(s.ShelterId))
                .Select(s =>
                {
                    var route = routesById[s.ShelterId];

                    return new RoutedShelter(
                        s,
                        route.Meters,
                        route.Seconds,
                        route.IsEstimate);
                })
                .Where(r => r.Seconds <= maxSeconds)
                .ToList();

            if (routed.Count == 0)
            {
                return NoResults(
                    byCar,
                    zone.AreaName,
                    alertType,
                    alertSource,
                    maxSeconds,
                    offerDriving,
                    "לא נמצא מקלט שאפשר להגיע אליו בזמן.");
            }

            // שלב 10: דירוג המקלטים לפי העדפות וזמן הגעה
            var ranked = _ranking.Rank(
                routed,
                required,
                optional,
                maxSeconds,
                _options.MaxResults);

            // שלב 11: בניית תוצאות החיפוש
            var results = ranked.Select((r, index) =>
            {
                var dto = _mapper.Map<ShelterSearchResultDTO>(
                    r.Routed.Shelter);

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

        // קביעת סוג ההתרעה:
        // התרעה פעילה משרת הדמה קודמת לברירת המחדל.
        private (string Type, string Source) ResolveAlertType(
            AlertZone zone)
        {
            var live = _alertCache.Current;

            if (live != null && live.Covers(zone.AreaName))
            {
                return (live.AlertType, "Live");
            }

            return (_options.DefaultAlertType, "Default");
        }

        // בניית תשובה כאשר לא נמצאו מקלטים מתאימים
        private ShelterSearchResponseDTO NoResults(
            bool byCar,
            string areaName,
            string alertType,
            string alertSource,
            int maxSeconds,
            bool offerDriving,
            string message)
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
