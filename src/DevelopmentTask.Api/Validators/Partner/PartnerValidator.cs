using FluentValidation;
using static DevelopmentTask.Api.Endpoints.Partner.RememberMeEndpoint;

namespace DevelopmentTask.Api.Validators.Partner
{
    public sealed class PartnerValidator : AbstractValidator<RememberMeQuery>
    {
        public PartnerValidator()
        {
            RuleFor(x => x.Code)
                .NotNull();
        }
    }
}
