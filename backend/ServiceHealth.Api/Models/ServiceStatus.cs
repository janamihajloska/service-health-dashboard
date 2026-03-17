namespace ServiceHealth.Api.Models;

public class ServiceStatus
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public HealthStatus Status { get; set; }
    public long ResponseTimeMs { get; set; }
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
}