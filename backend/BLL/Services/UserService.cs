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
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<UserProfileDTO> GetAll()
        {
            var users = _userRepository.GetAll();
            return _mapper.Map<List<UserProfileDTO>>(users);
        }
        public UserProfileDTO? GetById(int id)
        {
            var user = _userRepository.GetById(id);
            return user == null ? null : _mapper.Map<UserProfileDTO>(user);
        }

        //פונקציות הרשמה והתחברות



    }
}
