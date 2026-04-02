using Microsoft.Extensions.Diagnostics.HealthChecks;
using DevelopmentTask.Infrastructure.Persistence;

namespace DevelopmentTask.Infrastructure.Common
{
    internal sealed class DatabaseHealthCheck(AppDbContext ctx) : IHealthCheck
    {
        private readonly AppDbContext _ctx = ctx;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
            => await _ctx.Database.CanConnectAsync(cancellationToken)
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
    }
}
