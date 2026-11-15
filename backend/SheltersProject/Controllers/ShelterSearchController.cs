using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterSearchController : ControllerBase
    {
        private readonly IShelterSearchService _service;

        public ShelterSearchController(IShelterSearchService service)
        {
            _service = service;
        }

        // POST: api/ShelterSearch   (פתוח לאורח; משתמש רשום מזוהה לפי הטוקן אם נשלח)
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ShelterSearchResponseDTO>> Search(
            [FromBody] ShelterSearchRequestDTO dto, CancellationToken ct)
        {
            // בלי טוקן תקף: null = אורח, והסינון לפי מיקום בלבד
            var userId = User.GetUserIdOrNull();
            return Ok(await _service.SearchAsync(dto, userId, ct));
        }
    }
}