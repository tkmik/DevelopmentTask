using System.Reflection;
using DevelopmentTask.Api.Common.Options.HealthCheck;
using DevelopmentTask.Api.Endpoints.Journal;
using DevelopmentTask.Api.Endpoints.Tree;
using DevelopmentTask.Api.Filters;
using DevelopmentTask.Api.Validators.Journal;
using DevelopmentTask.Auth;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace DevelopmentTask.Api
{
    public static class ServiceCollectionExtensions
    {
        private const string baseRoute = "/api/user";

        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(options =>
                {
                    options.OperationFilter<CorrelationIdHeaderFilter>();
                    options.AddAuthSecurity();
                })
                .AddValidatorsFromAssemblyContaining<Program>()
                .AddCustomValidators()
                .AddHealthCheck(configuration);


            return services;
        }

        public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapJournalEndpoints(baseRoute)
                .MapTreeEndpoints(baseRoute)
                .MapTreeNodeEndpoints(baseRoute)
                .MapPartnerEndpoints(baseRoute);

            return routeBuilder;
        }

        private static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<HealthCheckConfigureOptions>();

            services.AddHealthChecks();

            var sp = services.BuildServiceProvider();
            var config = sp.GetRequiredService<IOptions<HealthCheckConfig>>()!.Value;

            var healthChecksUi = services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(config.PollingInterval!.Value);
                options.AddHealthCheckEndpoint(config.ServiceName, config.Endpoint);
            });

            if (string.IsNullOrWhiteSpace(config.PostgreConnection))
            {
                healthChecksUi.AddInMemoryStorage();
            }
            else
            {
                healthChecksUi.AddPostgreSqlStorage(config.PostgreConnection);
            }

            return services;
        }

        public static WebApplication MapHealthCheck(this WebApplication app)
        {
            var config = app.Services.GetService<IOptions<HealthCheckConfig>>()?.Value;
            var endpoint = config?.Endpoint ?? HealthCheckConfigureOptions.DefaultEndpoint;

            app.MapHealthChecks(endpoint, new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.MapHealthChecksUI();

            return app;
        }

        private static IServiceCollection AddCustomValidators(this IServiceCollection services)
        {
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(a => a.Name.EndsWith("Validator") && !a.IsAbstract && !a.IsInterface)
                    .Select(a => new { assignedType = a })
                    .ToList()
                    .ForEach(typesToRegister =>
                    {
                        services.AddScoped(Type.GetType(typesToRegister.assignedType.BaseType!.AssemblyQualifiedName!)!, typesToRegister.assignedType);
                    });

            return services;
        }
    }
}
