using DevelopmentTask.Api.Endpoints.TreeNode;

namespace DevelopmentTask.Api.Endpoints.Tree
{
    public static class TreeNodeEndpoints
    {
        private const string treeNodeRoute = "/tree/node";

        public static IEndpointRouteBuilder MapTreeNodeEndpoints(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            var route = $"{baseRoute}{treeNodeRoute}";

            routeBuilder
                .MapCreateNodeEndpoint(route)
                .MapDeleteNodeEndpoint(route)
                .MapRenameNodeEndpoint(route);

            return routeBuilder;
        }
    }
}
