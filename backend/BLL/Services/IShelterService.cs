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

        // ציבורי: רק מקלטים מאושרים ופעילים
        List<ShelterSearchResultDTO> GetAll();
        ShelterSearchResultDTO? GetById(int id);

        //בעל מקלט או מנהל
        //ShelterDetailsDTO? GetDetails(int id, int userId, bool isAdmin);

        //בעל מקלט- משתמש מחובר שהמקלט שלו
        List<SharedShelterDTO> GetMyShelters(int userId);
        ShelterDetailsDTO? GetMyShelter(int id, int userId);
        ShelterDetailsDTO Add(int userId, ShelterRequestDTO dto);
        ShelterDetailsDTO? Update(int id, int userId, ShelterRequestDTO dto);
        ShelterDetailsDTO? UpdateAvailability(int id, int userId, UpdateShelterAvailabilityDTO dto);
        bool Delete(int id, int userId);

        // למנהל
        List<AdminShelterDTO> GetByStatus(string? status);
        ShelterDetailsDTO? UpdateStatus(int id, UpdateShelterStatusDTO dto);



          
    }
}
