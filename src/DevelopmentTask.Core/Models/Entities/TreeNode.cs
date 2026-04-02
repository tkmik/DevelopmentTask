using System.ComponentModel.DataAnnotations;

namespace DevelopmentTask.Core.Models.Entities
{
    public sealed class TreeNode : Entity<long>
    {
        public Guid TreeId { get; set; }

        public long? ParentId { get; set; }
        public TreeNode? Parent { get; set; }

        public List<TreeNode> Children { get; set; } = new();

        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
    }
}
