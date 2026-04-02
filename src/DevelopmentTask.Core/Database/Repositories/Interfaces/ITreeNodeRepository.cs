using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Core.Database.Repositories.Interfaces
{
    public interface ITreeNodeRepository : IEntityFrameworkRepository<TreeNode, long>
    {
        Task<ICollection<TreeNode>> GetDescendantsAsync(long nodeId, CancellationToken cancellationToken = default);
    }
}
