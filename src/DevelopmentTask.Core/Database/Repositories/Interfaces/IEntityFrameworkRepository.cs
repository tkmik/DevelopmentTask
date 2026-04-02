using System.Linq.Expressions;

namespace DevelopmentTask.Core.Database.Repositories.Interfaces
{
    public interface IEntityFrameworkRepository<TEntity, TId>
    {
        Task<IEntityFrameworkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

        Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);

        Task<List<TEntity>?> GetAsync(
            IEnumerable<string>? includeProperties = null,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? skipCount = null,
            int? takeCount = null,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter,
            IEnumerable<string>? includeProperties = null,
            CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
