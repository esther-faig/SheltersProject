using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly IPreferenceService _service;

        public PreferencesController(IPreferenceService service)
        {
            _service = service;
        }

        // GET: api/Preferences
        // מחזיר את כל ההעדפות במערכת (פתוח גם לאורח, כי מסך ההרשמה צריך אותן)
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<PreferenceDTO>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/Preferences/5  (לא קיימת: 404 מה-Filter)
        [HttpGet("{id:int}")]
        public ActionResult<PreferenceDTO> GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        // POST: api/Preferences  (מנהל בלבד; שם חובה נבדק ב-DTO ע"י [Required])
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<PreferenceDTO> Add([FromBody] PreferenceDTO dto)
        {
            var newPreference = _service.Add(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newPreference.PreferenceId },
                newPreference
            );
        }

        // PUT: api/Preferences/5  (מנהל בלבד)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public ActionResult<PreferenceDTO> Update(int id, [FromBody] PreferenceDTO dto)
        {
            return Ok(_service.Update(id, dto));
        }

        // DELETE: api/Preferences/5  (מנהל בלבד; בשימוש: 409, לא קיימת: 404)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}