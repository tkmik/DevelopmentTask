using DevelopmentTask.Api.Middlewares;

namespace DevelopmentTask.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationHeader(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }
    }
}
