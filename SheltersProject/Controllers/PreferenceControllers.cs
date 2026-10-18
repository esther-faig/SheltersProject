using BLL.Services;
using DTO;
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
        // מחזיר את כל ההעדפות במערכת
        [HttpGet]
        public ActionResult<List<PreferenceDTO>> GetAll()
        {
            var preferences = _service.GetAll();
            return Ok(preferences);
        }

        // GET: api/Preferences/5
        // מחזיר העדפה לפי מזהה
        [HttpGet("{id:int}")]
        public ActionResult<PreferenceDTO> GetById(int id)
        {
            var preference = _service.GetById(id);
            if (preference == null)
            {
                return NotFound();
            }

            return Ok(preference);
        }

        // POST: api/Preferences
        // מוסיף העדפה חדשה למערכת
        [HttpPost]
        public ActionResult<PreferenceDTO> Add([FromBody] PreferenceDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("יש להזין שם להעדפה.");
            }

            var newPreference = _service.Add(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newPreference.PreferenceId },
                newPreference
            );
        }

        // PUT: api/Preferences/5
        // מעדכן העדפה קיימת
        [HttpPut("{id:int}")]
        public ActionResult<PreferenceDTO> Update(int id, [FromBody] PreferenceDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("יש להזין שם להעדפה.");
            }

            var updatedPreference = _service.Update(id, dto);
            if (updatedPreference == null)
            {
                return NotFound();
            }

            return Ok(updatedPreference);
        }

        // DELETE: api/Preferences/5
        // מוחק העדפה לפי מזהה
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            bool deleted = _service.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}