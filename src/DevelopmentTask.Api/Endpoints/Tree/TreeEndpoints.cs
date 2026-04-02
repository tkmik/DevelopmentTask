using DevelopmentTask.Api.Endpoints.TreeNode;

namespace DevelopmentTask.Api.Endpoints.Tree
{
    public static class TreeEndpoints
    {
        private const string treeRoute = "/tree";

        public static IEndpointRouteBuilder MapTreeEndpoints(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            var route = $"{baseRoute}{treeRoute}";

            routeBuilder
                .MapGetEndpoint(route);

            return routeBuilder;
        }
    }
}
