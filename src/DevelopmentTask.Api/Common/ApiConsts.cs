namespace DevelopmentTask.Api.Common
{
    public static class ApiConsts
    {
        public const string CorrelationIdHeaderName = "X-Correlation-Id";

        public static class HealthCheck
        {
            public const string ConnectionStringName = "HealthCheckConnection";
            public const string Name = "DevelopmentTaskService";
            public const string Endpoint = "/health";
            public const int PollingInterval = 60;
        }
    }
}
