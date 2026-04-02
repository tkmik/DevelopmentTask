using DevelopmentTask.Api.Common.Extensions;
using DevelopmentTask.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevelopmentTask.Api.Endpoints.Tree
{
    public static class GetEndpoint
    {
        private const string getRoute = "/get";

        public static IEndpointRouteBuilder MapGetEndpoint(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            routeBuilder
                .MapPost($"{baseRoute}{getRoute}", HandleAsync)
                .WithValidationDefaults<DevelopmentTask.Core.Models.Entities.TreeNode, GetQuery>(
                    name: "tree.get",
                    @tag: "user.tree",
                    description: "Returns your entire tree. If your tree doesn't exist it will be created automatically.",
                    summary: "Reutrns entire tree.");

            return routeBuilder;
        }

        public static async Task<IResult> HandleAsync(
            [FromServices] ITreeService service,
            [AsParameters] GetQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await service.GetOrCreateAsync(query.TreeName, cancellationToken);

            return Results.Ok(result);
        }

        public class GetQuery
        {
            public required string TreeName { get; set; }
        }
    }
}
