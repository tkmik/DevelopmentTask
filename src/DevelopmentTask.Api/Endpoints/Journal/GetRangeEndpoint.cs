using DevelopmentTask.Api.Common.Extensions;
using DevelopmentTask.Core.Models.Dto.ExceptionJournal;
using DevelopmentTask.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevelopmentTask.Api.Endpoints.Journal
{
    public static class GetRangeEndpoint
    {
        private const string getRangeRoute = "/getRange";

        public static IEndpointRouteBuilder MapGetRange(this IEndpointRouteBuilder routeBuilder, string baseRoute)
        {
            routeBuilder
                .MapPost($"{baseRoute}{getRangeRoute}", HandleAsync)
                .WithValidationDefaults<ExceptionJournalInfo, GetRangeQuery, GetRangeBody>(
                    name: "journal.getRange",
                    @tag: "user.journal",
                    description: "Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.",
                    summary: "Returns journal errors.");

            return routeBuilder;
        }

        public static async Task<IResult> HandleAsync(
            [FromServices] IExceptionJournalService service,
            [AsParameters] GetRangeQuery query,
            [FromBody] GetRangeBody? body,
            CancellationToken cancellationToken = default)
        {
            var model = ToDto(query, null);
            var result = await service.GetAsync(model, cancellationToken);

            return Results.Ok(result);
        }

        public class GetRangeQuery
        {
            public int Skip { get; set; }
            public int Take { get; set; }
        }

        public class GetRangeBody
        {
            public long? Id { get; set; }
            public long? EventId { get; set; }
            public Guid? CorrelationId { get; set; }
            public string? RequestPath { get; set; }
            public string? HttpMethod { get; set; }
            public string? ExceptionMessage { get; set; }
            public string? StackTrace { get; set; }
            public string? QueryParams { get; set; }
            public string? BodyParams { get; set; }
        }

        private static ExceptionJournalFilter ToDto(GetRangeQuery query, GetRangeBody? body)
        {
            return new ExceptionJournalFilter
            {
                Skip = query.Skip,
                Take = query.Take,
                Id = body?.Id,
                EventId = body?.EventId,
                CorrelationId = body?.CorrelationId,
                RequestPath = body?.RequestPath,
                HttpMethod = body?.HttpMethod,
                ExceptionMessage = body?.ExceptionMessage,
                StackTrace = body?.StackTrace,
                QueryParams = body?.QueryParams,
                BodyParams = body?.BodyParams,
            };
        }
    }
}
