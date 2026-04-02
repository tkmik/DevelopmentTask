using System.Runtime.CompilerServices;

namespace DevelopmentTask.Api.Middlewares
{
    internal static partial class CorrelationIdMiddlewareLogger
    {
        [LoggerMessage(
            1,
            LogLevel.Information,
            "{callingMethod}: Starting request with CorrelationId: {correlationId}")]
        public static partial void StartingRequest(
            this ILogger<CorrelationIdMiddleware> logger,
            string correlationId,
            [CallerMemberName]
            string callingMethod = "");

        [LoggerMessage(
            2,
            LogLevel.Information,
            "{callingMethod}: Finishing request with CorrelationId: {correlationId}")]
        public static partial void FinishingRequest(
            this ILogger<CorrelationIdMiddleware> logger,
            string correlationId,
            [CallerMemberName]
            string callingMethod = "");
    }
}
