using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AlertStatusController : ControllerBase
    {
        private readonly IActiveAlertCache _cache;
        public AlertStatusController(IActiveAlertCache cache) => _cache = cache;

        [HttpGet]
        public ActionResult<AlertCacheStatus> Get() => Ok(_cache.GetStatus());
    }
}
