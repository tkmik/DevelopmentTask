using DevelopmentTask.Api.Common.Extensions;
using DevelopmentTask.Core.Models.Entities;
using DevelopmentTask.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevelopmentTask.Api.Endpoints.Journal
{
    public static class GetSingleEndpoint
    {
        private const string getSingleRoute = "/getSingle";

        public static IEndpointRouteBuilder MapGetSingle(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            routeBuilder
                .MapPost($"{baseRoute}{getSingleRoute}", HandleAsync)
                .WithValidationDefaults<ExceptionJournal, GetSingleQuery>(
                    name: "journal.getSingle",
                    @tag: "user.journal",
                    description: "Returns the information about an particular event by ID.",
                    summary: "Return info about journal error by id.")
                .Produces(StatusCodes.Status404NotFound);

            return routeBuilder;
        }

        public static async Task<IResult> HandleAsync(
            [FromServices] IExceptionJournalService service,
            [AsParameters] GetSingleQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await service.GetAsync(query.Id, cancellationToken);

            return result is null ? Results.NotFound() : Results.Ok(result);
        }

        public class GetSingleQuery
        {
            public required long Id { get; set; }
        }
    }
}
