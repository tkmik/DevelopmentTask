using Microsoft.AspNetCore.Mvc;
using DevelopmentTask.Api.Common.Extensions;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Api.Endpoints.TreeNode
{
    public static class DeleteNodeEndpoint
    {
        private const string createRoute = "/delete";

        public static IEndpointRouteBuilder MapDeleteNodeEndpoint(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            routeBuilder
                .MapPost($"{baseRoute}{createRoute}", HandleAsync)
                .WithValidationDefaults<DeleteNodeQuery>(
                    name: "tree.node.delete",
                    @tag: "user.tree.node",
                    description: "Delete an existing node and all its descendants.",
                    summary: "Delete a node.");

            return routeBuilder;
        }

        public static async Task<IResult> HandleAsync(
            [FromServices] ITreeService service,
            [AsParameters] DeleteNodeQuery query,
            CancellationToken cancellationToken = default)
        {
            await service.DeleteNodeAsync(query.NodeId, cancellationToken);

            return Results.Ok();
        }

        public class DeleteNodeQuery
        {
            public required long NodeId { get; set; }
        }
    }
}
