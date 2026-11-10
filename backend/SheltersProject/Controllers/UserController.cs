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

        // GET: api/User/me
        // הפרופיל שלי + ההעדפות שלי + המקלטים ששיתפתי (מסך 6)
        [HttpGet("me")]
        public ActionResult<UserProfileDTO> GetMe()
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

        // GET: api/User/5 - פרופיל משתמש לפי מזהה (מסך 6 / כללי)
        [HttpGet("{id:int}")]
        public ActionResult<UserProfileDTO> GetById(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT: api/User/5
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