using AutoMapper;
using DAL.Repositories;
using DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IShelterService
    {


        List<ShelterSearchResultDTO> GetAll();

        ShelterSearchResultDTO? GetById(int id);
        ShelterDetailsDTO Add(int userId, ShelterRequestDTO dto);
        ShelterDetailsDTO? Update(int id, ShelterRequestDTO dto);
        bool Delete(int id);
          
    }
}
