using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using DAL.Repositories;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IShelterFeatureService
    {
        List<ShelterFeature> GetAll();
        ShelterFeature? GetById(int id);
        ShelterFeature Add(ShelterFeature entity);
        ShelterFeature? Update(int id, ShelterFeature entity);
        bool Delete(int id);
    }
}
