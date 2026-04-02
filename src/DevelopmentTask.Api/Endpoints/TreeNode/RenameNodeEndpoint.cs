using Microsoft.AspNetCore.Mvc;
using DevelopmentTask.Api.Common.Extensions;
using DevelopmentTask.Core.Models.Dto.TreeNode;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Api.Endpoints.TreeNode
{
    public static class RenameNodeEndpoint
    {
        private const string createRoute = "/rename";

        public static IEndpointRouteBuilder MapRenameNodeEndpoint(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            routeBuilder
                .MapPost($"{baseRoute}{createRoute}", HandleAsync)
                .WithValidationDefaults<RenameNodeQuery>(
                    name: "tree.node.rename",
                    @tag: "user.tree.node",
                    description: "Rename an existing node in your tree. A new name of the node must be unique across all siblings.",
                    summary: "Rename a node.");

            return routeBuilder;
        }

        public static async Task<IResult> HandleAsync(
            [FromServices] ITreeService service,
            [AsParameters] RenameNodeQuery query,
            CancellationToken cancellationToken = default)
        {
            var model = ToDto(query);
            await service.UpdateNodeAsync(model, cancellationToken);

            return Results.Ok();
        }

        public class RenameNodeQuery
        {
            public required long NodeId { get; set; }
            public required string NewNodeName { get; set; }
        }

        private static RenameTreeNode ToDto(RenameNodeQuery query)
        {
            return new RenameTreeNode(query.NodeId, query.NewNodeName);
        }
    }
}
