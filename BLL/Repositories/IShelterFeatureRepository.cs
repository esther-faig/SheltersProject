using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public interface IShelterFeatureRepository
    {
        List<ShelterFeatureDTO> GetAll();
        ShelterFeatureDTO? GetById(int id);
        ShelterFeatureDTO Add(ShelterFeatureDTO dto);
        ShelterFeatureDTO? Update(int id, ShelterFeatureDTO dto);
        bool Delete(int id);
    }
}
