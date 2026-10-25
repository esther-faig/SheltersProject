using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTO;
using DAL;
namespace BLL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ───────────────────────── Preference ─────────────────────────
            CreateMap<Preference, PreferenceDTO>();

            CreateMap<PreferenceDTO, Preference>()
                .ForMember(d => d.PreferenceId, o => o.Ignore())              // Identity
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description ?? ""))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category ?? ""))
                .ForMember(d => d.UserPreferences, o => o.Ignore())
                .ForMember(d => d.ShelterFeatures, o => o.Ignore());

            // ───────────────────────── ShelterFeature ─────────────────────
            // Name / Description / Category מגיעים מטבלת Preference
            CreateMap<ShelterFeature, ShelterFeatureDTO>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Preference.Name))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Preference.Description))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Preference.Category))
                .ForMember(d => d.IsAvailable, o => o.MapFrom(s => s.IsAvailable ?? false));

            CreateMap<ShelterFeatureDTO, ShelterFeature>()
                .ForMember(d => d.ShelterFeatureId, o => o.Ignore())
                .ForMember(d => d.ShelterId, o => o.Ignore())                 // לא קיים ב-DTO, נקבע ב-Repository
                .ForMember(d => d.Shelter, o => o.Ignore())
                .ForMember(d => d.Preference, o => o.Ignore());

            // הלקוח שולח רק PreferenceId + IsAvailable בהוספה/עריכת מקלט
            CreateMap<ShelterFeatureUpdateDTO, ShelterFeature>()
                .ForMember(d => d.ShelterFeatureId, o => o.Ignore())
                .ForMember(d => d.ShelterId, o => o.Ignore())
                .ForMember(d => d.Shelter, o => o.Ignore())
                .ForMember(d => d.Preference, o => o.Ignore());

            // ───────────────────────── UserPreference ─────────────────────
            CreateMap<UserPreference, UserPreferenceDTO>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Preference.Name));

            CreateMap<UserPreferenceDTO, UserPreference>()
                .ForMember(d => d.UserPreferenceId, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore())                    // נקבע ב-Repository
                .ForMember(d => d.User, o => o.Ignore())
                .ForMember(d => d.Preference, o => o.Ignore());

            // ───────────────────────── Shelter → DTOs ─────────────────────
            // DistanceMeters ו-TravelTimeSeconds מחושבים בשירות החיפוש (Google Maps), לא ממופים
            CreateMap<Shelter, ShelterSearchResultDTO>()
                .ForMember(d => d.Features, o => o.MapFrom(s => s.ShelterFeatures))
                .ForMember(d => d.DistanceMeters, o => o.Ignore())
                .ForMember(d => d.TravelTimeSeconds, o => o.Ignore());

            CreateMap<Shelter, ShelterDetailsDTO>()
                .ForMember(d => d.Features, o => o.MapFrom(s => s.ShelterFeatures));

            // Status (enum) -> string בהמרה אוטומטית
            CreateMap<Shelter, AdminShelterDTO>()
                .ForMember(d => d.Features, o => o.MapFrom(s => s.ShelterFeatures));

            // IsActive ממופה לפי שם (דורש להוסיף IsActive ל-Shelter ב-DAL)
            CreateMap<Shelter, SharedShelterDTO>();

            // ───────────────────────── DTOs → Shelter ─────────────────────
            CreateMap<ShelterRequestDTO, Shelter>()
                .ForMember(d => d.ShelterFeatures, o => o.MapFrom(s => s.Features))
                .ForMember(d => d.ShelterId, o => o.Ignore())
                .ForMember(d => d.Status, o => o.Ignore())                    // ברירת מחדל Pending, משתנה רק ע"י מנהל
                .ForMember(d => d.AlertZoneId, o => o.Ignore())               // נקבע ע"י point-in-polygon
                .ForMember(d => d.AlertZone, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())           // נקבע מהמשתמש המחובר
                .ForMember(d => d.IsActive, o => o.Ignore())              // נקבע ב-Service
                .ForMember(d => d.RejectionReason, o => o.Ignore())       // נקבע ע"י מנהל
                .ForMember(d => d.User, o => o.Ignore());



            // החלטת מנהל: Status מגיע כמחרוזת. RejectionReason אין לו שדה ב-DAL
            CreateMap<UpdateShelterStatusDTO, Shelter>(MemberList.None);

            // בעל המקלט מפעיל/מבטל את המקלט (IsActive לפי שם)
            CreateMap<UpdateShelterAvailabilityDTO, Shelter>(MemberList.None);

            // ───────────────────────── User ───────────────────────────────
            CreateMap<User, UserProfileDTO>()
                .ForMember(d => d.Preferences, o => o.MapFrom(s => s.UserPreferences))
                .ForMember(d => d.SharedShelters, o => o.MapFrom(s => s.Shelters));

            CreateMap<User, UserResponseDto>()
                .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()))
                .ForMember(d => d.Token, o => o.Ignore());                    // JWT נוצר אחרי המיפוי

            // Password לא ממופה בכוונה: חייבים להצפין (hash) בשירות לפני שמירה
            CreateMap<RegisterRequestDTO, User>()
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.Role, o => o.MapFrom(_ => UserRole.Resident))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.Shelters, o => o.Ignore())
                .ForMember(d => d.UserPreferences, o => o.Ignore());

            CreateMap<UpdateUserRequestDTO, User>()
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.Role, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.Shelters, o => o.Ignore())
                .ForMember(d => d.UserPreferences, o => o.Ignore());          // העדפות מטופלות ב-Repository

            // ללא מיפוי (אין להם מקבילה ב-DAL): LoginRequestDTO, ShelterSearchRequestDTO,
            // ShelterSearchResponseDTO, ShelterEligibilityDTO, ShelterRankingDTO,
            // EmergencyGuidanceDTO, SaveUserPreferencesRequestDTO
            // AlertZone ו-ProtectionTime: ה-Repositories עובדים ישירות עם ישויות ה-DAL
        
            //IGNORE כשהשדה לא מתמלא ממי ששולח אלא ממקור אחר
            //FORMEMBER כשהמיפוי נעשה עם שמות של שדות לא זהים
            //MAPFORM כשהשמות שונים או כשהערך נמצא באובייקט מקושר
        }
    }
}
