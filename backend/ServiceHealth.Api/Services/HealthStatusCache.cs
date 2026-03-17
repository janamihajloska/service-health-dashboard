using ServiceHealth.Api.Models;

namespace ServiceHealth.Api.Services
{
    public class HealthStatusCache
    {
        private readonly Dictionary<string, Queue<ServiceStatus>> _history = new();
        private const int MaxHistory = 60;

        public IEnumerable<ServiceStatus> GetLatestStatuses()
        {
            return _history.Values
                .Where(q => q.Any())
                .Select(q => q.Last());
        }

        public IEnumerable<ServiceStatus> GetHistory(string serviceName)
        {
            if (!_history.ContainsKey(serviceName))
                return Enumerable.Empty<ServiceStatus>();

            return _history[serviceName];
        }

        public void UpdateStatuses(IEnumerable<ServiceStatus> statuses)
        {
            foreach (var status in statuses)
            {
                if (!_history.ContainsKey(status.Name))
                {
                    _history[status.Name] = new Queue<ServiceStatus>();
                }

                var queue = _history[status.Name];

                var newEntry = new ServiceStatus
                {
                    Name = status.Name,
                    Url = status.Url,
                    ResponseTimeMs = status.ResponseTimeMs,
                    CheckedAt = DateTime.UtcNow
                };

                queue.Enqueue(newEntry);

                if (queue.Count > MaxHistory)
                    queue.Dequeue();

                //Compute status based on history
                var avg = queue.Average(x => x.ResponseTimeMs);
                var last = queue.Last();

                if (avg == 0 || avg < 0)
                {
                    last.Status = HealthStatus.Down;
                }
                else if (avg > 2000)
                {
                    last.Status = HealthStatus.Degraded;
                }
                else
                {
                    last.Status = HealthStatus.Healthy;
                }
            }
        }
    }
}
