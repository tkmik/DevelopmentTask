using FluentValidation;
using static DevelopmentTask.Api.Endpoints.TreeNode.RenameNodeEndpoint;

namespace DevelopmentTask.Api.Validators.TreeNode
{
    public sealed class RenameNodeValidator : AbstractValidator<RenameNodeQuery>
    {
        public RenameNodeValidator()
        {
            RuleFor(x => x.NodeId)
                .GreaterThan(0);
            RuleFor(x => x.NewNodeName)
                .NotNull();
        }
    }
}
