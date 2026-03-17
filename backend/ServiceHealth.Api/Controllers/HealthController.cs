using Microsoft.AspNetCore.Mvc;
using ServiceHealth.Api.Models;
using ServiceHealth.Api.Services;

namespace ServiceHealth.Api.Controllers;

[ApiController]
[Route("api/v1/health")]
public class HealthController : ControllerBase
{
    private readonly HealthStatusCache _cache;

    public HealthController(HealthStatusCache cache)
    {
        _cache = cache;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_cache.GetLatestStatuses());
    }

    [HttpGet("{serviceName}/history")]
    public IActionResult GetHistory(string serviceName)
    {
        return Ok(_cache.GetHistory(serviceName));
    }

    [HttpGet("summary")]
    public IActionResult GetSummary()
    {
        var services = _cache.GetLatestStatuses();

        var summary = new
        {
            total = services.Count(),
            healthy = services.Count(s => s.Status == HealthStatus.Healthy),
            degraded = services.Count(s => s.Status == HealthStatus.Degraded),
            down = services.Count(s => s.Status == HealthStatus.Down)
        };

        return Ok(summary);
    }
}
