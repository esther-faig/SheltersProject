using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
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

// ------------------ בעל המקלט: המקלטים שלי ------------------
        // GET: api/Shelter
        [HttpGet]
        public ActionResult<List<ShelterSearchResultDTO>> GetAll()
        {
            var shelters = _service.GetAll();
            return Ok(shelters);
        }

        // GET: api/Shelter/my
        [HttpGet("my")]
        public ActionResult<List<SharedShelterDTO>> GetMyShelters()
        {
            return Ok(_service.GetMyShelters(User.GetUserId()));
        }
        // GET: api/Shelter/my/5
        // המקלט שלי לעריכה (גם אם עוד לא אושר)
        [HttpGet("my/{id:int}")]
        public ActionResult<ShelterDetailsDTO> GetMyShelter(int id)
        {
            var shelter = _service.GetMyShelter(id, User.GetUserId());
            if (shelter == null)
                return NotFound();

            return Ok(shelter);
        }

        // POST: api/Shelter
        // שיתוף מקלט חדש. הבעלים הוא המשתמש המחובר, והמקלט ממתין לאישור מנהל
        [HttpPost]
        public ActionResult<ShelterDetailsDTO> Add([FromForm] ShelterRequestDTO dto)
        {
        // מחזיר 404 למקלט כזה, מקלט חדש ממתין לאישור, ו-Ok ל-GetById
        try 
        { 
            return Ok(_service.Add(User.GetUserId(), dto)); 
        }
        catch (ArgumentException ex) 
        { 
        return BadRequest(ex.Message); 
        }
        }


        // PUT: api/Shelter/5
        // עריכה: רק הבעלים
        [HttpPut("{id:int}")]
        public ActionResult<ShelterDetailsDTO> Update(int id, [FromForm] ShelterRequestDTO dto)
        {
            try
            {
                var updated = _service.Update(id, User.GetUserId(), dto);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex) 
            { 
                return BadRequest(ex.Message); 
            }
        }
        

        // PUT: api/Shelter/5/availability
        // בעל המקלט מבטל או מפעיל אותו זמנית
        [HttpPut("{id:int}/availability")]
        public ActionResult<ShelterDetailsDTO> UpdateAvailability(int id, [FromBody] UpdateShelterAvailabilityDTO dto)
        {
            var updated = _service.UpdateAvailability(id, User.GetUserId(), dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
        // PUT: api/Shelter/5/features
        // תיקון מאפיינים של מקלט שנטען מראש (למשל אחרי ביקור שם).
        // מקלט ששיתפה משתמשת אחרת מחזיר 404, כלומר אי אפשר לשנות אותו
        [HttpPut("{id:int}/features")]
        public ActionResult<ShelterDetailsDTO> UpdateImportedFeatures(int id, [FromBody] UpdateShelterFeaturesDTO dto)
        {
            
            
            var updated = _service.UpdateImportedFeatures(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);  
        }


        // DELETE: api/Shelter/5
        // מחיקה: רק הבעלים
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_service.Delete(id, User.GetUserId()))
                return NotFound();

            return NoContent();
        }

        // ───────────── מנהל בלבד ─────────────

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
        // אישור או דחייה של מקלט שהמשתמשים שיתפו
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}/status")]
        public ActionResult<ShelterDetailsDTO> UpdateStatus(int id, [FromBody] UpdateShelterStatusDTO dto)
        {
            var updated = _service.UpdateStatus(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }


        //search-
        // [AllowAnonymous]
        // [HttpPost("search")]
        // public ActionResult<ShelterSearchResponseDTO> Search([FromBody] ShelterSearchRequestDTO dto)
        // {
        //     return Ok(_service.Search(dto, User.GetUserIdOrNull()));
        // }

    }
}