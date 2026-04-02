using Microsoft.EntityFrameworkCore;
using DevelopmentTask.Core.Database.Repositories;
using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Infrastructure.Persistence.Repositories
{
    internal sealed class TreeNodeRepository(AppDbContext ctx)
        : EntityFrameworkRepository<AppDbContext, TreeNode, long>(ctx), ITreeNodeRepository
    {
        public async Task<ICollection<TreeNode>> GetDescendantsAsync(long nodeId, CancellationToken cancellationToken = default)
        {
            var root = await ctx.TreeNodes.AsNoTracking().FirstAsync(n => n.Id == nodeId , cancellationToken);

            return await ctx.TreeNodes.AsNoTracking().Where(x => x.Id != root.Id && x.Path.StartsWith(root.Path)).ToListAsync(cancellationToken);
        }
    }
}
