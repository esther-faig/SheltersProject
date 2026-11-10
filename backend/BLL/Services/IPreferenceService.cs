using DTO;

namespace BLL.Services
{
    public interface IPreferenceService
    {
        List<PreferenceDTO> GetAll();
        PreferenceDTO GetById(int id);
        PreferenceDTO Add(PreferenceDTO dto);
        PreferenceDTO Update(int id, PreferenceDTO dto);
        void Delete(int id);
    }
}