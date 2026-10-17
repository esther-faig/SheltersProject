
using BLL.Repositories;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectionTimesController : ControllerBase
    {
        private readonly IProtectionTimeRepository _repository;

        public ProtectionTimesController(IProtectionTimeRepository repository)
        {
            _repository = repository;
        }

        // GET: api/ProtectionTimes
        [HttpGet]
        public ActionResult<List<ProtectionTime>> GetAll()
        {
            var protectionTimes = _repository.GetAll();
            return Ok(protectionTimes);
        }

        // GET: api/ProtectionTimes/5
        [HttpGet("{id}")]
        public ActionResult<ProtectionTime> GetById(int id)
        {
            var protectionTime = _repository.GetById(id);

            if (protectionTime == null)
                return NotFound();

            return Ok(protectionTime);
        }

        // POST: api/ProtectionTimes
        [HttpPost]
        public ActionResult<ProtectionTime> Add(ProtectionTime entity)
        {
            var addedProtectionTime = _repository.Add(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = addedProtectionTime.ProtectionTimeId },
                addedProtectionTime
            );
        }

        // PUT: api/ProtectionTimes/5
        [HttpPut("{id}")]
        public ActionResult<ProtectionTime> Update(int id, ProtectionTime entity)
        {
            var updatedProtectionTime = _repository.Update(id, entity);

            if (updatedProtectionTime == null)
                return NotFound();

            return Ok(updatedProtectionTime);
        }

        // DELETE: api/ProtectionTimes/5
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
