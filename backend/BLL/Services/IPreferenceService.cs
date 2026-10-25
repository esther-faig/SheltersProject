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
    public interface IPreferenceService
    {
        List<PreferenceDTO> GetAll();
        PreferenceDTO? GetById(int id);
        PreferenceDTO Add(PreferenceDTO dto);
        PreferenceDTO? Update(int id, PreferenceDTO dto);
        bool Delete(int id);
    }
}
