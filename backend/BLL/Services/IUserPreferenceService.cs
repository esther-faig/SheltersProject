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
    public interface IUserPreferenceService
    {
        List<UserPreferenceDTO> GetAll();
        UserPreferenceDTO? GetById(int id);
        List<UserPreferenceDTO> GetByUserId(int userId);
        void SaveUserPreferences(int userId, SaveUserPreferencesRequestDTO dto);

    }
}
