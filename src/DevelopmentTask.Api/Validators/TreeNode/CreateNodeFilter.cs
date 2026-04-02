using FluentValidation;
using static DevelopmentTask.Api.Endpoints.TreeNode.CreateNodeEndpoint;

namespace DevelopmentTask.Api.Validators.TreeNodes
{
    public sealed class CreateNodeFilter : AbstractValidator<CreateNodeQuery>
    {
        public CreateNodeFilter()
        {
            RuleFor(x => x.TreeName)
                .NotNull();
            RuleFor(x => x.ParentNodeId)
                .GreaterThan(0)
                .When(x => x.ParentNodeId.HasValue)
                .WithMessage($"{nameof(CreateNodeQuery.ParentNodeId)} must be greater than 0 when provided.");
            RuleFor(x => x.NodeName)
                .NotNull();
        }
    }
}
