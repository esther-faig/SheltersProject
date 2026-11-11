using DTO;

namespace BLL.Services
{
    // מקלט/משתמש שלא נמצאו: זורקים NotFoundException (לא מחזירים null)
    public interface IShelterService
    {
        // ציבורי: רק מקלטים מאושרים ופעילים
        List<ShelterSearchResultDTO> GetAll();
        ShelterSearchResultDTO GetById(int id);

        //בעל מקלט- משתמש מחובר שהמקלט שלו
        List<SharedShelterDTO> GetMyShelters(int userId);
        ShelterDetailsDTO GetMyShelter(int id, int userId);
        ShelterDetailsDTO Add(int userId, ShelterRequestDTO dto);
        ShelterDetailsDTO Update(int id, int userId, ShelterRequestDTO dto);
        ShelterDetailsDTO UpdateAvailability(int id, int userId, UpdateShelterAvailabilityDTO dto);
        void Delete(int id, int userId);

        // למנהל
        List<AdminShelterDTO> GetByStatus(string? status);
        ShelterDetailsDTO UpdateStatus(int id, UpdateShelterStatusDTO dto);

        //לאיגנט
        List<ShelterSearchResultDTO> SearchByCity(string city, int max);   // בממשק

    }
}