using FluentValidation;
using static DevelopmentTask.Api.Endpoints.Journal.GetSingleEndpoint;

namespace DevelopmentTask.Api.Validators.Journal
{
    public class GetSingleValidator : AbstractValidator<GetSingleQuery>
    {
        public GetSingleValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
