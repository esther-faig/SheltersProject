
using BLL.Repositories;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferenceController : ControllerBase
    {
        private readonly IUserPreferenceRepository _repository;

        public UserPreferenceController(
            IUserPreferenceRepository repository)
        {
            _repository = repository;
        }

        // GET: api/UserPreference
        [HttpGet]
        public ActionResult<List<UserPreferenceDTO>> GetAll()
        {
            var preferences = _repository.GetAll();
            return Ok(preferences);
        }

        // GET: api/UserPreference/5
        [HttpGet("{id}")]
        public ActionResult<ShelterSearchResultDTO> GetById(int id)
        {
            var preference = _repository.GetById(id);

            if (preference == null)
                return NotFound();

            return Ok(preference);
        }

        // POST: api/UserPreference
        [HttpPost]
        public ActionResult<ShelterSearchResultDTO> Add(
            [FromBody] ShelterSearchResultDTO dto)
        {
            var addedPreference = _repository.Add(dto);
            return Ok(addedPreference);
        }

        // PUT: api/UserPreference/5
        [HttpPut("{id}")]
        public ActionResult<UserPreferenceDTO> Update(
            int id,
            [FromBody] ShelterSearchResultDTO dto)
        {
            var updatedPreference = _repository.Update(id, dto);

            if (updatedPreference == null)
                return NotFound();

            return Ok(updatedPreference);
        }

        // DELETE: api/UserPreference/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = _repository.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
