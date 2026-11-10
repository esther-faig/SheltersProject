using AutoMapper;
using BLL.Exceptions;
using DAL.Repositories;
using DTO;

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

        public UserProfileDTO GetById(int id)
        {
            var user = _userRepository.GetById(id)
                ?? throw new NotFoundException($"משתמש {id} לא נמצא");
            return _mapper.Map<UserProfileDTO>(user);
        }

        //פונקציות הרשמה והתחברות
        //לממש. בהרשמה: אם GetByEmail מצא משתמש -> throw new ConflictException("כתובת המייל כבר רשומה")
        public UserResponseDTO Register(RegisterRequestDTO dto)
        {
            return null;
        }
        public UserResponseDTO? Login(LoginRequestDTO dto)
        {
            return null;
        }

        public UserProfileDTO Update(int userId, UpdateUserRequestDTO dto)
        {
            var user = _userRepository.GetById(userId)
                ?? throw new NotFoundException($"משתמש {userId} לא נמצא");

            var email = dto.Email.Trim().ToLowerInvariant();

            var sameEmail = _userRepository.GetByEmail(email);
            if (sameEmail != null && sameEmail.UserId != userId)
                throw new ConflictException("כבר קיים משתמש אחר עם המייל הזה");

            // רק שם ומייל. הסיסמה, התפקיד וההעדפות לא נוגעים
            user.FullName = dto.FullName;
            user.Email = email;
            user.UpdatedAt = DateTime.UtcNow;

            return _mapper.Map<UserProfileDTO>(_userRepository.Update(user));
        }
    }
}