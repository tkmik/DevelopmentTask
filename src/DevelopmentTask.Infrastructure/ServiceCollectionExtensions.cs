using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Infrastructure.Common;
using DevelopmentTask.Infrastructure.Persistence;
using DevelopmentTask.Infrastructure.Persistence.Interceptors;
using DevelopmentTask.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DevelopmentTask.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRepositories()
                .AddInterceptors()
                .AddDbContext<AppDbContext>(
                    (serviceProvider, contextOptions) =>
                    {
                        contextOptions.UseNpgsql(configuration.GetConnectionString(AppDbContext.ConnectionStringName));
                        contextOptions.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>()!);
                    });

            services.TryAddSingleton(TimeProvider.System);

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("postgresql");

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.TryAddScoped<IExceptionJournalRepository, ExceptionJournalRepository>();
            services.TryAddScoped<ITreeNodeRepository, TreeNodeRepository>();
            services.TryAddScoped<ITreeRepository, TreeRepository>();
            services.TryAddScoped<IUserRepository, UserRepository>();

            return services;
        }

        private static IServiceCollection AddInterceptors(this IServiceCollection services)
        {
            services.TryAddSingleton<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            return services;
        }
    }
}