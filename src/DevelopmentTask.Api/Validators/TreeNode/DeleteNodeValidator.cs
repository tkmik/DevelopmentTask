using FluentValidation;
using static DevelopmentTask.Api.Endpoints.TreeNode.DeleteNodeEndpoint;

namespace DevelopmentTask.Api.Validators.TreeNodes
{
    public sealed class DeleteNodeValidator : AbstractValidator<DeleteNodeQuery>
    {
        public DeleteNodeValidator()
        {
            RuleFor(x => x.NodeId)
                .GreaterThan(0);
        }
    }
}
