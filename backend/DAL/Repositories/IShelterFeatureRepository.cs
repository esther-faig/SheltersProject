using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IShelterFeatureRepository
    {
        List<ShelterFeature> GetAll();
        ShelterFeature? GetById(int id);
        ShelterFeature Add(ShelterFeature entity);
        public List<ShelterFeature> GetByShelterId(int shelterId);

        ShelterFeature? Update(ShelterFeature entity);
        bool Delete(int id);
    }
}
