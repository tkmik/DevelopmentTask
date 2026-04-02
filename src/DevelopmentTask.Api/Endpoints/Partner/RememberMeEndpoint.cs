using DevelopmentTask.Api.Common.Extensions;
using DevelopmentTask.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevelopmentTask.Api.Endpoints.Partner
{
    public static class RememberMeEndpoint
    {
        private const string getRoute = "/rememberMe";

        public static IEndpointRouteBuilder MapRememberMeEndpoint(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            routeBuilder
                .MapPost($"{baseRoute}{getRoute}", Handle)
                .WithValidationDefaults<TokenInfo, RememberMeQuery>(
                    name: "user.rememberMe",
                    @tag: "user.partner",
                    description: "(Optional) Saves user by unique code and returns auth token required on all other requests, if implemented.",
                    summary: "Returns auth token for further requests.",
                    requireAuthorization: false);

            return routeBuilder;
        }

        public static async Task<IResult> Handle(
            [FromServices] IAuthenticationService service,
            [AsParameters] RememberMeQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await service.AuthenticateAsync(query.Code, cancellationToken);

            return Results.Ok(new TokenInfo(result));
        }

        public sealed class RememberMeQuery
        {
            public required string Code { get; set; }
        }

        public sealed record TokenInfo(string Token);
    }
}
