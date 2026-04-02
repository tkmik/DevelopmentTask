using Microsoft.Extensions.DependencyInjection;

namespace DevelopmentTask.Infrastructure.Common.Extensions
{
    internal static class DbContextExtensions
    {
        public static IServiceCollection AddAutoApplyingMigrations(this IServiceCollection services)
        {


            return services;
        }
    }
}
