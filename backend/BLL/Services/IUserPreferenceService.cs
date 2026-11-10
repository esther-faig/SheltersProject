using DTO;

namespace BLL.Services
{
    public interface IUserPreferenceService
    {
        List<UserPreferenceDTO> GetAll();
        UserPreferenceDTO GetById(int id);
        List<UserPreferenceDTO> GetByUserId(int userId);
        void SaveUserPreferences(int userId, SaveUserPreferencesRequestDTO dto);
    }
}