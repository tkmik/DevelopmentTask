using System.Runtime.CompilerServices;

namespace DevelopmentTask.Api.Middlewares
{
    internal static partial class ExceptionMiddlewareLog
    {
        [LoggerMessage(
            1,
            LogLevel.Error,
            "{callingMethod}: Operation was cancelled while writing exception journal entry.")]
        public static partial void OperationCancelledError(
            this ILogger<ExceptionMiddleware> logger,
            Exception? exception = null,
            [CallerMemberName]
            string callingMethod = "");

        [LoggerMessage(
           2,
           LogLevel.Error,
           "{callingMethod}: An exception occurred while writing exception journal entry.")]
        public static partial void UnexpectedError(
           this ILogger<ExceptionMiddleware> logger,
           Exception? exception = null,
           [CallerMemberName]
            string callingMethod = "");
    }
}
