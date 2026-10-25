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
    public class UserService : IUserService
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
        //לממש
        public UserResponseDto Register(RegisterRequestDTO dto)
        {
            return null;
        }
        public UserResponseDto? Login(LoginRequestDTO dto)
        {
            return null;
        }


        //לבדוק
        public UserProfileDTO? Update(int userId, UpdateUserRequestDTO dto)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
                return null;

            var email = dto.Email.Trim().ToLowerInvariant();

            var sameEmail = _userRepository.GetByEmail(email);
            if (sameEmail != null && sameEmail.UserId != userId)
                throw new InvalidOperationException("כבר קיים משתמש אחר עם המייל הזה");

            // רק שם ומייל. הסיסמה, התפקיד וההעדפות לא נוגעים
            user.FullName = dto.FullName;
            user.Email = email;
            user.UpdatedAt = DateTime.UtcNow;

            return _mapper.Map<UserProfileDTO>(_userRepository.Update(user));
        }


    }
}
