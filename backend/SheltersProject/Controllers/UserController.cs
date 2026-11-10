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
            return Ok(_service.GetAll());
        }

        // GET: api/User/5
        // פרופיל משתמש (מסך 6)
        [HttpGet("{id:int}")]
        public ActionResult<UserProfileDTO> GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        // כאן יתווספו בהמשך: הרשמה (Register) והתחברות (Login), כשיהיו ב-IUserService

        // PUT: api/User/5
        [HttpPut("{id:int}")]
        public ActionResult<UserProfileDTO> Update(int id, [FromBody] UpdateUserRequestDTO dto)
        {
            return Ok(_service.Update(id, dto));
        }
    }
}