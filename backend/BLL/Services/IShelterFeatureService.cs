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
        List<ShelterFeatureDTO> GetAll();
        ShelterFeatureDTO? GetById(int id);
        ShelterFeatureDTO Add(ShelterFeatureDTO dto);
        ShelterFeatureDTO? Update(int id, ShelterFeatureDTO dto);
        bool Delete(int id);
    }
}
