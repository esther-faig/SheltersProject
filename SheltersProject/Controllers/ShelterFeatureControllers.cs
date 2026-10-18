using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterFeaturesController : ControllerBase
    {
        private readonly IShelterFeatureService _service;

        public ShelterFeaturesController(IShelterFeatureService service)
        {
            _service = service;
        }

        // GET: api/ShelterFeatures
        [HttpGet]
        public ActionResult<List<ShelterFeatureDTO>> GetAll()
        {
            var features = _service.GetAll();
            return Ok(features);
        }

        // GET: api/ShelterFeatures/5
        [HttpGet("{id:int}")]
        public ActionResult<ShelterFeatureDTO> GetById(int id)
        {
            var feature = _service.GetById(id);
            if (feature == null)
                return NotFound();

            return Ok(feature);
        }

        // POST: api/ShelterFeatures
        [HttpPost]
        public ActionResult<ShelterFeatureDTO> Add([FromBody] ShelterFeatureDTO dto)
        {
            var addedFeature = _service.Add(dto);
            return Ok(addedFeature);
        }

        // PUT: api/ShelterFeatures/5
        [HttpPut("{id:int}")]
        public ActionResult<ShelterFeatureDTO> Update(int id, [FromBody] ShelterFeatureDTO dto)
        {
            var updatedFeature = _service.Update(id, dto);
            if (updatedFeature == null)
                return NotFound();

            return Ok(updatedFeature);
        }

        // DELETE: api/ShelterFeatures/5
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