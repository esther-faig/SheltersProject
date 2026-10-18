using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/Shelters
        [HttpGet]
        public ActionResult<List<ShelterSearchResultDTO>> GetAll()
        {
            var shelters = _service.GetAll();
            return Ok(shelters);
        }

        // GET: api/Shelters/5
        [HttpGet("{id:int}")]
        public ActionResult<ShelterSearchResultDTO> GetById(int id)
        {
            var shelter = _service.GetById(id);
            if (shelter == null)
                return NotFound();

            return Ok(shelter);
        }

        // POST: api/Shelters?userId=5
        // זמני: בשלב ההרשאות ה-userId יילקח מה-token ולא מה-URL
        [HttpPost]
        public ActionResult<ShelterDetailsDTO> Add([FromQuery] int userId, [FromBody] ShelterRequestDTO dto)
        {
            var addedShelter = _service.Add(userId, dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = addedShelter.ShelterId },
                addedShelter
            );
        }

        // PUT: api/Shelters/5
        [HttpPut("{id:int}")]
        public ActionResult<ShelterDetailsDTO> Update(int id, [FromBody] ShelterRequestDTO dto)
        {
            var updatedShelter = _service.Update(id, dto);
            if (updatedShelter == null)
                return NotFound();

            return Ok(updatedShelter);
        }

        // DELETE: api/Shelters/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var deleted = _service.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}