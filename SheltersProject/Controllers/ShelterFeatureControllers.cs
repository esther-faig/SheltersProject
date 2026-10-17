
using BLL.Repositories;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterFeaturesController : ControllerBase
    {
        private readonly IShelterFeatureRepository _repository;

        public ShelterFeaturesController(
            IShelterFeatureRepository repository)
        {
            _repository = repository;
        }

        // GET: api/ShelterFeatures
        [HttpGet]
        public ActionResult<List<ShelterFeatureDTO>> GetAll()
        {
            var features = _repository.GetAll();
            return Ok(features);
        }

        // GET: api/ShelterFeatures/5
        [HttpGet("{id}")]
        public ActionResult<ShelterFeatureDTO> GetById(int id)
        {
            var feature = _repository.GetById(id);

            if (feature == null)
                return NotFound();

            return Ok(feature);
        }

        // POST: api/ShelterFeatures
        [HttpPost]
        public ActionResult<ShelterFeatureDTO> Add(
            [FromBody] ShelterFeatureDTO dto)
        {
            var addedFeature = _repository.Add(dto);

            return Ok(addedFeature);
        }

        // PUT: api/ShelterFeatures/5
        [HttpPut("{id}")]
        public ActionResult<ShelterFeatureDTO> Update(
            int id,
            [FromBody] ShelterFeatureDTO dto)
        {
            var updatedFeature = _repository.Update(id, dto);

            if (updatedFeature == null)
                return NotFound();

            return Ok(updatedFeature);
        }

        // DELETE: api/ShelterFeatures/5
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
