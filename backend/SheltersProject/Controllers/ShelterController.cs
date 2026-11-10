using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SheltersProject.Controllers
{
    // אין כאן try/catch: שגיאות נתפסות ב-GlobalExceptionFilter, ונתונים לא תקינים נעצרים ע"י [ApiController]
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterController : ControllerBase
    {
        private readonly IShelterService _service;

        public ShelterController(IShelterService service)
        {
            _service = service;
        }

        // GET: api/Shelter  (פתוח גם לאורח שלא נרשם)
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<ShelterSearchResultDTO>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/Shelter/5  (פתוח גם לאורח)
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public ActionResult<ShelterSearchResultDTO> GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        // GET: api/Shelter/my  (המקלטים שלי, ה-userId מהטוקן)
        [HttpGet("my")]
        public ActionResult<List<SharedShelterDTO>> GetMyShelters()
        {
            return Ok(_service.GetMyShelters(User.GetUserId()));
        }

        // GET: api/Shelter/my/5  (המקלט שלי לעריכה, גם אם עוד לא אושר)
        [HttpGet("my/{id:int}")]
        public ActionResult<ShelterDetailsDTO> GetMyShelter(int id)
        {
            return Ok(_service.GetMyShelter(id, User.GetUserId()));
        }

        // POST: api/Shelter
        [HttpPost]
        public ActionResult<ShelterDetailsDTO> Add([FromForm] ShelterRequestDTO dto)
        {
            // Ok ולא CreatedAtAction: מקלט חדש ממתין לאישור, ו-GetById מחזיר 404 למקלט כזה
            return Ok(_service.Add(User.GetUserId(), dto));
        }

        // PUT: api/Shelter/5
        [HttpPut("{id:int}")]
        public ActionResult<ShelterDetailsDTO> Update(int id, [FromForm] ShelterRequestDTO dto)
        {
            return Ok(_service.Update(id, User.GetUserId(), dto));
        }

        // PUT: api/Shelter/5/availability
        [HttpPut("{id:int}/availability")]
        public ActionResult<ShelterDetailsDTO> UpdateAvailability(int id, [FromBody] UpdateShelterAvailabilityDTO dto)
        {
            return Ok(_service.UpdateAvailability(id, User.GetUserId(), dto));
        }

        // DELETE: api/Shelter/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id, User.GetUserId());
            return NoContent();
        }

        // ---------- למנהל בלבד ----------

        // GET: api/Shelter/admin?status=Pending   (בלי status = כל המקלטים)
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public ActionResult<List<AdminShelterDTO>> GetByStatus(
            [FromQuery]
            [RegularExpression("^(Pending|Approved|Rejected)$", ErrorMessage = "סטטוס לא תקין")]
            string? status)
        {
            return Ok(_service.GetByStatus(status));
        }

        // PUT: api/Shelter/5/status
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}/status")]
        public ActionResult<ShelterDetailsDTO> UpdateStatus(int id, [FromBody] UpdateShelterStatusDTO dto)
        {
            return Ok(_service.UpdateStatus(id, dto));
        }
    }
}