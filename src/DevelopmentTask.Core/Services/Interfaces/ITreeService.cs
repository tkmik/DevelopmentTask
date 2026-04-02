using DevelopmentTask.Core.Models.Dto.TreeNode;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Core.Services.Interfaces
{
    public interface ITreeService
    {
        Task CreateNodeAsync(CreateTreeNode node, CancellationToken cancellationToken = default);
        Task DeleteNodeAsync(long nodeId, CancellationToken cancellationToken = default);
        Task<TreeNode> GetOrCreateAsync(string treeName, CancellationToken cancellationToken = default);
        Task UpdateNodeAsync(RenameTreeNode node, CancellationToken cancellationToken = default);
    }
}
