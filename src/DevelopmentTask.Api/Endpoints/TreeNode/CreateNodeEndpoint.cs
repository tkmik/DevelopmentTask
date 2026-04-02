using Microsoft.AspNetCore.Mvc;
using DevelopmentTask.Api.Common.Extensions;
using DevelopmentTask.Core.Models.Dto.TreeNode;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Api.Endpoints.TreeNode
{
    public static class CreateNodeEndpoint
    {
        private const string createRoute = "/create";

        public static IEndpointRouteBuilder MapCreateNodeEndpoint(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            routeBuilder
                .MapPost($"{baseRoute}{createRoute}", HandleAsync)
                .WithValidationDefaults<CreateNodeQuery>(
                    name: "tree.node.create",
                    @tag: "user.tree.node",
                    description: "Create a new node in your tree. You must to specify a parent node ID that belongs to your tree or dont pass parent ID to create tree first level node. A new node name must be unique across all siblings.",
                    summary: "Create a new node.");

            return routeBuilder;
        }

        public static async Task<IResult> HandleAsync(
            [FromServices] ITreeService service,
            [AsParameters] CreateNodeQuery query,
            CancellationToken cancellationToken = default)
        {
            var node = ToDto(query);

            await service.CreateNodeAsync(node, cancellationToken);

            return Results.Ok();
        }

        public class CreateNodeQuery
        {
            public required string TreeName { get; set; }
            public long? ParentNodeId { get; set; }
            public required string NodeName { get; set; }
        }

        private static CreateTreeNode ToDto(CreateNodeQuery query)
            => new CreateTreeNode(query.TreeName, query.ParentNodeId, query.NodeName);
    }
}
