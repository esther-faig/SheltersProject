using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace SheltersProject.Controllers
{
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
            var shelters = _service.GetAll();
            return Ok(shelters);
        }

        // GET: api/Shelter/5
        [HttpGet("{id:int}")]
        public ActionResult<ShelterSearchResultDTO> GetById(int id)
        {
            var shelter = _service.GetById(id);
            if (shelter == null)
                return NotFound();

            return Ok(shelter);
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
            var shelter = _service.GetMyShelter(id, userId);
            if (shelter == null)
                return NotFound();

            return Ok(shelter);
        }

        // POST: api/Shelter?userId=5
        // זמני: בשלב ההרשאות ה-userId יילקח מה-token ולא מה-URL
        [HttpPost]
        public ActionResult<ShelterDetailsDTO> Add([FromQuery] int userId, [FromBody] ShelterRequestDTO dto)
        {
            // Ok ולא CreatedAtAction: מקלט חדש ממתין לאישור, ו-GetById מחזיר 404 למקלט כזה
            return Ok(_service.Add(userId, dto));
        }

        // PUT: api/Shelter/5
        [HttpPut("{id:int}")]
        public ActionResult<ShelterDetailsDTO> Update(int id, [FromQuery] int userId, [FromBody] ShelterRequestDTO dto)
        {
            var updated = _service.Update(id, userId, dto);
            if (updated == null)
                return NotFound();   // לא קיים או לא שלך: אותה תשובה

            return Ok(updated);
        }
        // PUT: api/Shelter/5/availability?userId=3
        // בעל המקלט מבטל או מפעיל אותו זמנית
        [HttpPut("{id:int}/availability")]
        public ActionResult<ShelterDetailsDTO> UpdateAvailability(int id, [FromQuery] int userId, [FromBody] UpdateShelterAvailabilityDTO dto)
        {
            var updated = _service.UpdateAvailability(id, userId, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }


        // DELETE: api/Shelter/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id, [FromQuery] int userId)
        {
            if (!_service.Delete(id, userId))
                return NotFound();

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
            var updated = _service.UpdateStatus(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

    }
}