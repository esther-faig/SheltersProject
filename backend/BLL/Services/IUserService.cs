using DTO;

namespace BLL.Services
{
    public interface IUserService
    {
        List<UserProfileDTO> GetAll();
        UserProfileDTO GetById(int id);                                   // לא קיים = NotFoundException

        UserResponseDTO Register(RegisterRequestDTO dto);                 // מייל תפוס = ConflictException
        UserResponseDTO? Login(LoginRequestDTO dto);                      // null = פרטים שגויים

        UserProfileDTO Update(int userId, UpdateUserRequestDTO dto);
    }
}