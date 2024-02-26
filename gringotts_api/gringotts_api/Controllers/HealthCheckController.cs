using Microsoft.AspNetCore.Mvc;

namespace gringotts_api.Controllers
{
    /// <summary>
    /// Controller for performing health checks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// Endpoint to perform a health check.
        /// </summary>
        /// <returns>OK response with a massage indicating the health status.</returns>
        [HttpGet]
        public IActionResult getCheck()
        {
            return Ok(new { Message = "OK" });
        }
    }
}
