using System.Text.Json;
using FluentValidation;
using static DevelopmentTask.Api.Endpoints.Journal.GetRangeEndpoint;

namespace DevelopmentTask.Api.Validators.Journal
{
    public class GetRangeBodyValidator : AbstractValidator<GetRangeBody>
    {
        private readonly HashSet<string> HttpMethodsSet = 
            [HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete, HttpMethods.Head, HttpMethods.Options, HttpMethods.Patch];

        public GetRangeBodyValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .When(x => x.Id.HasValue)
                .WithMessage($"{nameof(GetRangeBody.Id)} must be greater than 0 when provided.");
            RuleFor(x => x.EventId)
                .GreaterThan(0)
                .When(x => x.EventId.HasValue)
                .WithMessage($"{nameof(GetRangeBody.EventId)} must be greater than 0 when provided.");
            RuleFor(x => x.HttpMethod)
                .Must(x => string.IsNullOrWhiteSpace(x) || HttpMethodsSet.Contains(x, StringComparer.OrdinalIgnoreCase))
                .WithMessage($"{nameof(GetRangeBody.HttpMethod)} must be a valid HTTP method or null/empty.")
                .When(x => !string.IsNullOrWhiteSpace(x.HttpMethod));
            RuleFor(x => x.QueryParams)
                .Must(obj => IsValidJson(obj))
                .WithMessage($"{nameof(GetRangeBody.QueryParams)} cannot be serialized to JSON.")
                .When(x => !string.IsNullOrWhiteSpace(x.QueryParams));
            RuleFor(x => x.BodyParams)
                .Must(obj => IsValidJson(obj))
                .WithMessage($"{nameof(GetRangeBody.BodyParams)} cannot be serialized to JSON.")
                .When(x => !string.IsNullOrWhiteSpace(x.BodyParams));
        }

        private static bool IsValidJson(string? obj)
        {
            try
            {
                JsonSerializer.Serialize(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
