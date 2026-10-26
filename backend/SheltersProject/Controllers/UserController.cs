using BLL.Services;
using DTO;
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
        //[HttpGet]
        //public ActionResult<List<UserProfileDTO>> GetAll()
        //{
        //    var users = _service.GetAll();
        //    return Ok(users);
        //}


        // GET: api/User/5
        // פרופיל משתמש (מסך 6)
        [HttpGet("{id:int}")]
        public ActionResult<UserProfileDTO> GetById(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //הרשמה והתחברות    
        // POST api/user/register
        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDTO dto)
        {
            if (!_service.Register(dto))
                return Conflict("Email is already registered.");

            // ההרשמה רק יוצרת את המשתמש. את הטוקן מקבלים בהתחברות
            return Ok();
        }

        // POST api/user/login
        [HttpPost("login")]
        public ActionResult<AuthResponseDTO> Login(LoginRequestDTO dto)
        {
            var result = _service.Login(dto);
            if (result is null)
                return Unauthorized("Wrong email or password.");

            return Ok(result);
        }



        //להוסיף גם עדכון
        [HttpPut("{id:int}")]
        public ActionResult<UserProfileDTO> Update(int id, [FromBody] UpdateUserRequestDTO dto)
        {
            var updated = _service.Update(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }



    }
}