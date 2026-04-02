using DevelopmentTask.Core.Database.Repositories;
using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Infrastructure.Persistence.Repositories
{
    internal sealed class TreeRepository(AppDbContext ctx)
        : EntityFrameworkRepository<AppDbContext, Tree, Guid>(ctx), ITreeRepository
    {
    }
}
