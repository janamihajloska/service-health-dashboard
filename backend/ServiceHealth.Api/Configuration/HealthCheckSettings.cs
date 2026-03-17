namespace ServiceHealth.Api.Configuration
{
    public class HealthCheckSettings
    {
        public int MaxParallelChecks { get; set; } = 5;
        public int TimeoutSeconds { get; set; } = 10;
        public int DegradedThresholdMs { get; set; } = 2000;
    }
}
