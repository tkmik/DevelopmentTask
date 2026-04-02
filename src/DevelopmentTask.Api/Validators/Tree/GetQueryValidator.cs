using FluentValidation;
using static DevelopmentTask.Api.Endpoints.Tree.GetEndpoint;

namespace DevelopmentTask.Api.Validators.Tree
{
    public sealed class GetQueryValidator : AbstractValidator<GetQuery>
    {
        public GetQueryValidator()
        {
            RuleFor(x => x.TreeName)
                .NotNull();
        }
    }
}
