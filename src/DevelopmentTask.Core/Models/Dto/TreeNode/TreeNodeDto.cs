namespace DevelopmentTask.Core.Models.Dto.TreeNode
{
    public sealed record TreeNodeDto(long Id, Guid TreeId, long? ParentId, string Name);
}
