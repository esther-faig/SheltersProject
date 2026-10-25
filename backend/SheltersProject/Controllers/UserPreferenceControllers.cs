using BLL.Services;
using DTO;
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

        // GET: api/UserPreference
        [HttpGet]
        public ActionResult<List<UserPreferenceDTO>> GetAll()
        {
            var preferences = _service.GetAll();
            return Ok(preferences);
        }

        // GET: api/UserPreference/5
        [HttpGet("{id:int}")]
        public ActionResult<UserPreferenceDTO> GetById(int id)
        {
            var preference = _service.GetById(id);
            if (preference == null)
                return NotFound();

            return Ok(preference);
        }

        // GET: api/UserPreference/user/5
        // ההעדפות של משתמש מסוים (מסך 6)
        [HttpGet("user/{userId:int}")]
        public ActionResult<List<UserPreferenceDTO>> GetByUserId(int userId)
        {
            var preferences = _service.GetByUserId(userId);
            return Ok(preferences);
        }

        // PUT: api/UserPreference/user/5
        // שמירת ההעדפות של המשתמש (מסך 4 חלק שני, ומסך 6 "עריכת העדפות")
        [HttpPut("user/{userId:int}")]
        public IActionResult SaveUserPreferences(int userId, [FromBody] SaveUserPreferencesRequestDTO dto)
        {
            _service.SaveUserPreferences(userId, dto);
            return NoContent();
        }
    }
}