using DevelopmentTask.Api.Common;

namespace DevelopmentTask.Api.Middlewares
{
    public sealed class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var headerName = ApiConsts.CorrelationIdHeaderName;

            if (!context.Request.Headers.TryGetValue(headerName, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            context.Items[headerName] = correlationId;
            context.Response.Headers[headerName] = correlationId;

            _logger.StartingRequest(correlationId!);

            await _next(context);

            _logger.FinishingRequest(correlationId!);
        }
    }
}
