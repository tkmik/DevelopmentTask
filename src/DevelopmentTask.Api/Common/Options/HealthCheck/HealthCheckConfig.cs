namespace DevelopmentTask.Api.Common.Options.HealthCheck
{
    public sealed class HealthCheckConfig
    {
        public string ServiceName { get; set; } = null!;
        public string Endpoint { get; set; } = null!;
        public int? PollingInterval { get; set; } = null!;
        public string PostgreConnection { get; set; } = null!;
    }
}
