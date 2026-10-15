using BLL.Repositories;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertZonesController : ControllerBase
    {
        private readonly IAlertZoneRepository _alertZoneRepository;

        public AlertZonesController(IAlertZoneRepository alertZoneRepository)
        {
            _alertZoneRepository = alertZoneRepository;
        }

        // GET: api/AlertZones
        // מחזיר את כל אזורי ההתרעה
        [HttpGet]
        public ActionResult<List<AlertZone>> GetAll()
        {
            var alertZones = _alertZoneRepository.GetAll();

            return Ok(alertZones);
        }

        // GET: api/AlertZones/5
        // מחזיר אזור התרעה לפי מזהה
        [HttpGet("{id}")]
        public ActionResult<AlertZone> GetById(int id)
        {
            var alertZone = _alertZoneRepository.GetById(id);

            if (alertZone == null)
            {
                return NotFound();
            }

            return Ok(alertZone);
        }

        // POST: api/AlertZones
        // מוסיף אזור התרעה חדש
        [HttpPost]
        public ActionResult<AlertZone> Add([FromBody] AlertZone alertZone)
        {
            if (alertZone == null ||
                string.IsNullOrWhiteSpace(alertZone.AreaName))
            {
                return BadRequest("יש להזין שם לאזור ההתרעה.");
            }

            var newAlertZone = _alertZoneRepository.Add(alertZone);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newAlertZone.AlertZoneId },
                newAlertZone
            );
        }

        // PUT: api/AlertZones/5
        // מעדכן אזור התרעה קיים
        [HttpPut("{id}")]
        public ActionResult<AlertZone> Update(
            int id,
            [FromBody] AlertZone alertZone)
        {
            if (alertZone == null ||
                string.IsNullOrWhiteSpace(alertZone.AreaName))
            {
                return BadRequest("יש להזין שם לאזור ההתרעה.");
            }

            var updatedAlertZone =
                _alertZoneRepository.Update(id, alertZone);

            if (updatedAlertZone == null)
            {
                return NotFound();
            }

            return Ok(updatedAlertZone);
        }

        // DELETE: api/AlertZones/5
        // מוחק אזור התרעה לפי מזהה
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = _alertZoneRepository.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}