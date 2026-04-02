using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DevelopmentTask.Core.Services;
using DevelopmentTask.Core.Services.Interfaces;
using DevelopmentTask.Core.Services.Resolvers;

namespace DevelopmentTask.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services
                .AddServices()
                .AddSpecificationProviders();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddScoped<IExceptionJournalService, ExceptionJournalService>();
            services.TryAddScoped<ITreeService, TreeService>();
            services.TryAddScoped<IUserService, UserService>();
            services.TryAddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }

        private static IServiceCollection AddSpecificationProviders(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(ServiceCollectionExtensions))
                .AddClasses(classes => classes.AssignableTo(typeof(ISpecificationProvider<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.TryAddScoped<ISpecificationResolver, SpecificationResolver>();

            return services;
        }
    }
}
