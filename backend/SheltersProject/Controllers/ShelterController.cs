using BLL.Services;
using DTO;
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

        // GET: api/Shelter
        [HttpGet]
        public ActionResult<List<ShelterSearchResultDTO>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/Shelter/5
        [HttpGet("{id:int}")]
        public ActionResult<ShelterSearchResultDTO> GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        // GET: api/Shelter/my?userId=3
        [HttpGet("my")]
        public ActionResult<List<SharedShelterDTO>> GetMyShelters([FromQuery] int userId)
        {
            return Ok(_service.GetMyShelters(userId));
        }

        // GET: api/Shelter/my/5?userId=3
        // המקלט שלי לעריכה (גם אם עוד לא אושר)
        [HttpGet("my/{id:int}")]
        public ActionResult<ShelterDetailsDTO> GetMyShelter(int id, [FromQuery] int userId)
        {
            return Ok(_service.GetMyShelter(id, userId));
        }

        // POST: api/Shelter?userId=5
        // זמני: בשלב ההרשאות ה-userId יילקח מה-token ולא מה-URL
        [HttpPost]
        public ActionResult<ShelterDetailsDTO> Add([FromQuery] int userId, [FromForm] ShelterRequestDTO dto)
        {
            // Ok ולא CreatedAtAction: מקלט חדש ממתין לאישור, ו-GetById מחזיר 404 למקלט כזה
            return Ok(_service.Add(userId, dto));
        }

        // PUT: api/Shelter/5
        [HttpPut("{id:int}")]
        public ActionResult<ShelterDetailsDTO> Update(int id, [FromQuery] int userId, [FromForm] ShelterRequestDTO dto)
        {
            return Ok(_service.Update(id, userId, dto));
        }

        // PUT: api/Shelter/5/availability?userId=3
        // בעל המקלט מבטל או מפעיל אותו זמנית
        [HttpPut("{id:int}/availability")]
        public ActionResult<ShelterDetailsDTO> UpdateAvailability(int id, [FromQuery] int userId, [FromBody] UpdateShelterAvailabilityDTO dto)
        {
            return Ok(_service.UpdateAvailability(id, userId, dto));
        }

        // DELETE: api/Shelter/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id, [FromQuery] int userId)
        {
            _service.Delete(id, userId);
            return NoContent();
        }

        //למנהל
        // GET: api/Shelter/admin?status=Pending   (בלי status = כל המקלטים)
        [HttpGet("admin")]
        public ActionResult<List<AdminShelterDTO>> GetByStatus(
            [FromQuery]
            [RegularExpression("^(Pending|Approved|Rejected)$", ErrorMessage = "סטטוס לא תקין")]
            string? status)
        {
            return Ok(_service.GetByStatus(status));
        }

        // PUT: api/Shelter/5/status
        [HttpPut("{id:int}/status")]
        public ActionResult<ShelterDetailsDTO> UpdateStatus(int id, [FromBody] UpdateShelterStatusDTO dto)
        {
            return Ok(_service.UpdateStatus(id, dto));
        }
    }
}