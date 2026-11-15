using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectionTimeController : ControllerBase
    {
        private readonly IProtectionTimeImportService _import;

        public ProtectionTimeController(IProtectionTimeImportService import)
        {
            _import = import;
        }

        // POST: api/ProtectionTime/import  (מנהל בלבד: מושך את הזמנים משרת הפייתון למסד)
        [Authorize(Roles = "Admin")]
        [HttpPost("import")]
        public async Task<ActionResult<ImportResult>> Import(CancellationToken ct)
        {
            return Ok(await _import.ImportAsync(ct));
        }
    }
}