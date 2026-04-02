using DevelopmentTask.Api.Filters;

namespace DevelopmentTask.Api.Common.Extensions
{
    public static class EndpointExtensions
    {
        public static RouteHandlerBuilder WithValidationDefaults<TQueryValidation>(this RouteHandlerBuilder builder,
            string name,
            string tag,
            string description,
            string summary = "",
            bool requireAuthorization = true)
        {
            builder = builder.GetValidBuilderWithQueryValidation<TQueryValidation>(name, tag, description, summary, requireAuthorization);

            return builder
                .Produces(StatusCodes.Status200OK)
                .GetCommonHttpProducedError(requireAuthorization)
                .WithOpenApi();
        }

        public static RouteHandlerBuilder WithValidationDefaults<TResponse, TQueryValidation>(this RouteHandlerBuilder builder,
           string name,
           string tag,
           string description,
           string summary = "",
           bool requireAuthorization = true)
        {
            builder = builder.GetValidBuilderWithQueryValidation<TQueryValidation>(name, tag, description, summary, requireAuthorization);

            return builder
                .Produces<TResponse>(StatusCodes.Status200OK)
                .GetCommonHttpProducedError(requireAuthorization)
                .WithOpenApi();
        }


        public static RouteHandlerBuilder WithValidationDefaults<TResponse, TQueryValidation, TBodyValidation>(this RouteHandlerBuilder builder,
            string name,
            string tag,
            string description,
            string summary = "",
            bool requireAuthorization = true)
        {
            builder = builder.GetValidBuilderWithQueryAndBodyValidation<TQueryValidation, TBodyValidation>(name, tag, description, summary, requireAuthorization);

            return builder
                .Produces<TResponse>(StatusCodes.Status200OK)
                .GetCommonHttpProducedError(requireAuthorization)
                .WithOpenApi();
        }

        private static RouteHandlerBuilder GetValidBuilderWithQueryValidation<TQueryValidation>(this RouteHandlerBuilder builder,
            string name,
            string tag,
            string description,
            string summary,
            bool requireAuthorization)
        {
            return builder
                .AddEndpointFilter<ValidationFilter<TQueryValidation>>()
                .Produces(StatusCodes.Status400BadRequest)
                .GetValidBuilder(name, tag, description, summary, requireAuthorization);
        }

        private static RouteHandlerBuilder GetValidBuilderWithQueryAndBodyValidation<TQueryValidation, TBodyValidation>(this RouteHandlerBuilder builder,
           string name,
           string tag,
           string description,
           string summary,
           bool requireAuthorization)
        {
            return builder
                .GetValidBuilderWithQueryValidation<TQueryValidation>(name, tag, description, summary, requireAuthorization)
                .AddEndpointFilter<ValidationFilter<TBodyValidation>>();
        }

        private static RouteHandlerBuilder GetValidBuilder(this RouteHandlerBuilder builder,
            string name,
            string tag,
            string description,
            string summary,
            bool requireAuthorization)
        {
            if (requireAuthorization)
            {
                builder = builder.RequireAuthorization(policy => policy.RequireAuthenticatedUser());
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                builder = builder.WithName(name);
            }
            if (!string.IsNullOrWhiteSpace(tag))
            {
                builder = builder.WithTags(tag);
            }

            return builder
                .WithSummary(summary)
                .WithDescription(description);
        }

        private static RouteHandlerBuilder GetCommonHttpProducedError(this RouteHandlerBuilder builder, bool requireAuthorization)
        {
            if (requireAuthorization)
            {
                builder = builder
                    .Produces(StatusCodes.Status401Unauthorized);
            }

            return builder
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
