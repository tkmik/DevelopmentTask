using DevelopmentTask.Api.Endpoints.Partner;

namespace DevelopmentTask.Api.Endpoints.Tree
{
    public static class PartnerEndpoints
    {
        private const string treeRoute = "/partner";

        public static IEndpointRouteBuilder MapPartnerEndpoints(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            var route = $"{baseRoute}{treeRoute}";

            routeBuilder
                .MapRememberMeEndpoint(route);

            return routeBuilder;
        }
    }
}
