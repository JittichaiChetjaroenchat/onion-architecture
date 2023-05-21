using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _service;

        public HealthCheckController(HealthCheckService service)
        {
            _service = service;
        }

        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(HealthReport), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var report = await _service.CheckHealthAsync();

            if (report.Status == HealthStatus.Healthy)
                return Ok(report);
            return NotFound("Service unavailable");
        }
    }
}