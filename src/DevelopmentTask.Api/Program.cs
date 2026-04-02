using DevelopmentTask.Api.Extensions;
using DevelopmentTask.Auth;
using DevelopmentTask.Core;
using DevelopmentTask.Infrastructure;

namespace DevelopmentTask.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                builder.Services
                    .AddCommon()
                    .AddInfrastructure(builder.Configuration)
                    .AddAuth(builder.Configuration)
                    .AddWebApi(builder.Configuration);
            }           

            var app = builder.Build();
            {
                app.UseCorrelationHeader();
                app.UseGlobalExceptionHandler();

                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapApiEndpoints();

                app.MapHealthCheck();
            }           

            app.Run();
        }
    }
}
