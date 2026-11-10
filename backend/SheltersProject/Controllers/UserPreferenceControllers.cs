using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferenceController : ControllerBase
    {
        private readonly IUserPreferenceService _service;

        public UserPreferenceController(IUserPreferenceService service)
        {
            _service = service;
        }

        // GET: api/UserPreference  (מנהל בלבד: ההעדפות של כל המשתמשים)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<List<UserPreferenceDTO>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/UserPreference/me  (ההעדפות של המשתמש המחובר, מסך 6)
        [HttpGet("me")]
        public ActionResult<List<UserPreferenceDTO>> GetMine()
        {
            return Ok(_service.GetByUserId(User.GetUserId()));
        }

        // PUT: api/UserPreference/me  (מסך 4 חלק שני, ומסך 6 "עריכת העדפות")
        [HttpPut("me")]
        public IActionResult SaveMine([FromBody] SaveUserPreferencesRequestDTO dto)
        {
            _service.SaveUserPreferences(User.GetUserId(), dto);
            return NoContent();
        }

        // GET: api/UserPreference/5  (מנהל בלבד)
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public ActionResult<UserPreferenceDTO> GetById(int id)
        {
            return Ok(_service.GetById(id));
        }
    }
}