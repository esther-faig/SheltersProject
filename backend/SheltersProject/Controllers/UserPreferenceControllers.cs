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


        // GET: api/UserPreference/me
        [HttpGet("me")]
        public ActionResult<UserPreferenceDTO> GetMine()
        {
        
            var preferences = _service.GetByUserId(User.GetUserId());
            return Ok(preferences);
        }


        // PUT: api/UserPreference/me
        // שמירת ההעדפות של המשתמש (מסך 4 חלק שני, ומסך 6 "עריכת העדפות")
        // אפשר לשנות בכל זמן, למשל אחרי ביקור במקלט

        [HttpPut("me")]
        public IActionResult SaveMine([FromBody] SaveUserPreferencesRequestDTO dto)
        {
            _service.SaveUserPreferences(User.GetUserId(), dto);
            return NoContent();
        }



        // בכוונה אין כאן GetAll / GetById / user/{userId}:
        // הם חשפו העדפות (כולל צרכי נגישות ובריאות) של כל המשתמשים.
        // המנהל רואה העדפות של משתמשים דרך GET api/User
    }
}