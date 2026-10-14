using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public interface IPreferenceRepository
    {
        List<PreferenceDTO> GetAll();
        PreferenceDTO? GetById(int id);
        PreferenceDTO Add(PreferenceDTO dto);
        PreferenceDTO? Update(int id, PreferenceDTO dto);
        bool Delete(int id);

    }
}
