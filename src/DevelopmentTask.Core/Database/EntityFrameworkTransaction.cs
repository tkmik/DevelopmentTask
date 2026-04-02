using Microsoft.EntityFrameworkCore.Storage;
using DevelopmentTask.Core.Database.Repositories.Interfaces;

namespace DevelopmentTask.Core.Database
{
    internal class EntityFrameworkTransaction : IEntityFrameworkTransaction
    {
        private IDbContextTransaction _transaction;
        private bool _committed;
        private bool _disposedValue;

        public EntityFrameworkTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync(cancellationToken);
            _committed = true;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IDbContextTransaction? GetTransaction()
        {
            return _transaction;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_transaction is not null && !_committed)
                    {
                        _transaction!.Rollback();
                    }
                }

                _disposedValue = true;
            }
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_transaction is not null && !_committed)
                    {
                        await _transaction!.RollbackAsync();
                    }
                }

                _disposedValue = true;
            }
        }
    }
}
