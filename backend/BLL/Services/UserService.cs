using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using DAL.Repositories;
using DTO;
using Microsoft.AspNetCore.Identity;
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
        private readonly ITokenService _tokens;
        private readonly IPasswordHasher<User> _hasher;


        public UserService(IUserRepository userRepository, ITokenService tokens, IPasswordHasher<User> hasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokens = tokens;
            _hasher = hasher;
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
        public bool Register(RegisterRequestDTO dto)
        {
            //לא מביא טוקן
            var email = dto.Email.Trim().ToLowerInvariant();
            if (_userRepository.EmailExists(email))
                return false;

            // התפקיד נקבע בשרת, לא מגיע מהלקוח
            var user = _mapper.Map<User>(dto);
            user.Email = email;
            user.Role = UserRole.Resident;                     // תמיד נקבע בשרת
            user.Password = _hasher.HashPassword(user, dto.Password);            //הצפנת הסיסמה

            _userRepository.Add(user);
            return true; // הקונטרולר יחליט מה להחזיר ללקוח
        
        }
        public AuthResponseDTO? Login(LoginRequestDTO dto)
        {
            //מביא טוקן
            var email = dto.Email.Trim().ToLowerInvariant();
            var user = _userRepository.GetByEmail(email);
            if (user is null)
                return null;
            //פיענוח הסיסמה
            var result = _hasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return _tokens.CreateToken(user);
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
