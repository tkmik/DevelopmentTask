using System.Reflection;
using Microsoft.Extensions.Options;

namespace DevelopmentTask.Api.Common.Options.HealthCheck
{
    public sealed class HealthCheckConfigureOptions : IConfigureOptions<HealthCheckConfig>
    {
        public const string HealthCheckSectionName = "HealthCheck";

        public const string DefaultEndpoint = "/health";
        public const int DefaultPoolingInterval = 60;

        private readonly IConfiguration _configuration;

        public HealthCheckConfigureOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void Configure(HealthCheckConfig options)
        {
            var config = _configuration.GetSection(HealthCheckSectionName).Get<HealthCheckConfig>();

            var defaultName = Assembly.GetExecutingAssembly().GetName().Name!;

            if (config is null)
            {
                options.ServiceName = defaultName;
                options.Endpoint = DefaultEndpoint;
                options.PollingInterval = DefaultPoolingInterval;

                return;
            }
            
            options.ServiceName = !string.IsNullOrWhiteSpace(config.ServiceName) ? config.ServiceName : defaultName;
            options.Endpoint = !string.IsNullOrWhiteSpace(config.Endpoint) ? config.Endpoint : DefaultEndpoint;
            options.PollingInterval = config.PollingInterval.GetValueOrDefault(DefaultPoolingInterval);
            options.PostgreConnection = config.PostgreConnection ?? string.Empty;
        }
    }
}
