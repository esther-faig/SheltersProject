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
        [HttpGet]
        public ActionResult<List<UserProfileDTO>> GetAll()
        {
            var users = _service.GetAll();
            return Ok(users);
        }

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

        // כאן יתווספו בהמשך: הרשמה (Register) והתחברות (Login), כשיהיו ב-IUserService




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