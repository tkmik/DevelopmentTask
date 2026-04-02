namespace DevelopmentTask.Core.Models.Entities
{
    public sealed class Tree : Entity<Guid>
    {
        public required string Name { get; set; }
        public List<TreeNode> Nodes { get; set; } = new();
    }
}
