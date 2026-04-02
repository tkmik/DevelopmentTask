namespace DevelopmentTask.Core.Models.Dto.TreeNode
{
    public sealed record CreateTreeNode(string TreeName, long? ParentId, string NodeName);
}
