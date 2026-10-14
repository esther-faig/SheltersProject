using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public interface IUserPreferenceRepository
    {
        List<UserPreferenceDTO> GetAll();
        ShelterSearchResultDTO? GetById(int id);
        ShelterSearchResultDTO Add(ShelterSearchResultDTO dto);
        UserPreferenceDTO? Update(int id, ShelterSearchResultDTO dto);
        bool Delete(int id);
    }
}
