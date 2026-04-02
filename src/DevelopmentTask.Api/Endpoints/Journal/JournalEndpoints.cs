namespace DevelopmentTask.Api.Endpoints.Journal
{
    public static class JournalEndpoints
    {
        private const string journalRoute = "/journal";

        public static IEndpointRouteBuilder MapJournalEndpoints(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            var route = $"{baseRoute}{journalRoute}";

            routeBuilder.MapGetRange(route);
            routeBuilder.MapGetSingle(route);

            return routeBuilder;
        }
    }
}
