using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using DevelopmentTask.Core.Models.Entities;
using DevelopmentTask.Infrastructure.Extensions;

namespace DevelopmentTask.Infrastructure.Persistence.Interceptors
{
    internal sealed class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly TimeProvider _timeProvider;

        public AuditableEntityInterceptor(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? dbContext)
        {
            if (dbContext is null) return;

            foreach (var entity in dbContext.ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entity.State is EntityState.Added or EntityState.Modified || entity.HasChangedOwnedEntities())
                {
                    var utcNow = _timeProvider.GetUtcNow();

                    if (entity.State is EntityState.Added)
                    {
                        entity.Entity.CreatedAt = utcNow;
                    }

                    entity.Entity.ModifiedAt = utcNow;
                }
            }
        }
    }
}
