using Microsoft.EntityFrameworkCore.Storage;

namespace DevelopmentTask.Core.Database.Repositories.Interfaces
{
    public interface IEntityFrameworkTransaction : IDisposable, IAsyncDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);

        IDbContextTransaction? GetTransaction();
    }
}
