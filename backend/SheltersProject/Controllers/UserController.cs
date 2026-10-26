using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // GET: api/User
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public ActionResult<List<UserProfileDTO>> GetAll()
        {
            var users = _service.GetAll();
            return Ok(users);
        }


        // GET: api/User/5
        // הפרופיל שלי + ההעדפות שלי + המקלטים ששיתפתי (מסך 6)
        [HttpGet("me")]
        public ActionResult<UserProfileDTO> GetMe(int id)
        {
            var user = _service.GetById(User.GetUserId());
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT: api/User/me
        // עדכון שם ומייל של עצמי
        [HttpPut("me")]
        public ActionResult<UserProfileDTO> UpdateMe([FromBody] UpdateUserRequestDTO dto)
        {
            var updated = _service.Update(User.GetUserId(), dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        //הרשמה והתחברות    
        // POST api/user/register
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDTO dto)
        {
            if (!_service.Register(dto))
                return Conflict("Email is already registered.");

            // ההרשמה רק יוצרת את המשתמש. את הטוקן מקבלים בהתחברות
            return Ok();
        }

        // POST api/user/login
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthResponseDTO> Login(LoginRequestDTO dto)
        {
            var result = _service.Login(dto);
            if (result is null)
                return Unauthorized("Wrong email or password.");

            return Ok(result);
        }


    }
}