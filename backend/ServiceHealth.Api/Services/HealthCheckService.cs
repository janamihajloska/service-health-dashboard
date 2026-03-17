using Microsoft.Extensions.Options;
using ServiceHealth.Api.Configuration;
using ServiceHealth.Api.Models;
using System.Diagnostics;

namespace ServiceHealth.Api.Services
{
    public class HealthCheckService
    {
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _semaphore;
        private readonly HealthCheckSettings _settings;

        public HealthCheckService(HttpClient httpClient, IOptions<HealthCheckSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            _semaphore = new SemaphoreSlim(_settings.MaxParallelChecks);
        }

        public async Task<IEnumerable<ServiceStatus>> CheckServicesAsync(IEnumerable<ServiceStatus> services)
        {
            var tasks = services.Select(async service =>
            {
                await _semaphore.WaitAsync();

                var stopwatch = Stopwatch.StartNew();

                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, service.Url);
                    request.Headers.Add("User-Agent", "ServiceHealthChecker");

                    var cts = new CancellationTokenSource(
                        TimeSpan.FromSeconds(_settings.TimeoutSeconds));

                    var response = await _httpClient.SendAsync(request, cts.Token);

                }
                catch (TaskCanceledException)
                {
                    service.ResponseTimeMs = _settings.TimeoutSeconds * 1000;
                }
                catch
                {
                    service.ResponseTimeMs = -1;
                }
                finally
                {
                    stopwatch.Stop();

                    if (service.ResponseTimeMs == 0)
                        service.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

                    _semaphore.Release();
                }

                return service;
            });

            return await Task.WhenAll(tasks);
        }
    }
}