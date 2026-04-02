using FluentValidation;
using static DevelopmentTask.Api.Endpoints.Journal.GetRangeEndpoint;

namespace DevelopmentTask.Api.Validators.Journal
{
    public class GetRangeQueryValidator : AbstractValidator<GetRangeQuery>
    {
        public GetRangeQueryValidator()
        {
            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Take)
                .GreaterThanOrEqualTo(0);
        }
    }
}
