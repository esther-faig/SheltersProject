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
        List<Preference> GetAll();
        Preference? GetById(int id);
        Preference Add(Preference entity);
        Preference? Update(int id, Preference entity);
        bool Delete(int id);
    }
}
