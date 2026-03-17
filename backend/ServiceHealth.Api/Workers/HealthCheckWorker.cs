using Microsoft.Extensions.Options;
using ServiceHealth.Api.Configuration;
using ServiceHealth.Api.Models;
using ServiceHealth.Api.Services;

namespace ServiceHealth.Api.Workers;

public class HealthCheckWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly HealthStatusCache _cache;
    private readonly List<ServiceStatus> _services;

    public HealthCheckWorker(
        IServiceScopeFactory scopeFactory,
        HealthStatusCache cache,
        IOptions<List<ServiceConfig>> servicesConfig)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;

        // Initialize services ONCE
        _services = servicesConfig.Value
            .Select(s => new ServiceStatus
            {
                Name = s.Name,
                Url = s.Url
            })
            .ToList();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var healthCheckService = scope.ServiceProvider
                .GetRequiredService<HealthCheckService>();

            var services = _services.Select(s => new ServiceStatus
            {
                Name = s.Name,
                Url = s.Url
            });

            var results = await healthCheckService.CheckServicesAsync(services);

            _cache.UpdateStatuses(results);

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}