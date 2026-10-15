
using BLL.Repositories;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheltersController : ControllerBase
    {
        private readonly IShelterRepository _repository;

        public SheltersController(IShelterRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Shelters
        [HttpGet]
        public ActionResult<List<ShelterSearchResultDTO>> GetAll()
        {
            var shelters = _repository.GetAll();
            return Ok(shelters);
        }

        // GET: api/Shelters/5
        [HttpGet("{id}")]
        public ActionResult<ShelterSearchResultDTO> GetById(int id)
        {
            var shelter = _repository.GetById(id);

            if (shelter == null)
                return NotFound();

            return Ok(shelter);
        }

        // POST: api/Shelters
        [HttpPost]
        public ActionResult<ShelterSearchResultDTO> Add(
            [FromBody] ShelterSearchResultDTO dto)
        {
            var addedShelter = _repository.Add(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = addedShelter.ShelterId },
                addedShelter
            );
        }

        // PUT: api/Shelters/5
        [HttpPut("{id}")]
        public ActionResult<ShelterSearchResultDTO> Update(
            int id,
            [FromBody] ShelterSearchResultDTO dto)
        {
            var updatedShelter = _repository.Update(id, dto);

            if (updatedShelter == null)
                return NotFound();

            return Ok(updatedShelter);
        }

        // DELETE: api/Shelters/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _repository.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
