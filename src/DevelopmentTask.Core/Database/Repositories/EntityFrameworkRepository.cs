using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Core.Database.Repositories
{
    public abstract class EntityFrameworkRepository<TContext, TEntity, TId> : IEntityFrameworkRepository<TEntity, TId>
        where TEntity : Entity<TId>
        where TContext : DbContext
        where TId : notnull
    {
        private readonly TContext _ctx;

        public EntityFrameworkRepository(TContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEntityFrameworkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);

            return new EntityFrameworkTransaction(transaction);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await _ctx.Set<TEntity>().AddAsync(entity, cancellationToken);

            await _ctx.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await _ctx.Set<TEntity>().AnyAsync(filter, cancellationToken);
        }

        public async Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await _ctx.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<List<TEntity>?> GetAsync(
            IEnumerable<string>? includeProperties = null,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? skipCount = null,
            int? takeCount = null,
            CancellationToken cancellationToken = default)
        {
            var query = _ctx.Set<TEntity>().AsQueryable().AsNoTracking();

            foreach (string item in includeProperties ?? Enumerable.Empty<string>())
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    query = EntityFrameworkQueryableExtensions.Include(query, item);
                }
            }

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            query = orderBy is not null ? orderBy(query) : query;

            if (skipCount is not null && skipCount > 0)
            {
                query = query.Skip(skipCount.Value);
            }

            if (takeCount is not null && takeCount > 0)
            {
                query = query.Take(takeCount.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, IEnumerable<string>? includeProperties = null, CancellationToken cancellationToken = default)
        {
            var query = _ctx.Set<TEntity>().AsQueryable().AsNoTracking();

            foreach (string item in includeProperties ?? Enumerable.Empty<string>())
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    query = EntityFrameworkQueryableExtensions.Include(query, item);
                }
            }

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _ctx.Set<TEntity>().Remove(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _ctx.BulkDeleteAsync(entities.ToList(), cancellationToken: cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _ctx.Set<TEntity>().Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;

            await _ctx.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _ctx.BulkUpdateAsync(entities.ToList(), cancellationToken: cancellationToken);
        }
    }
}
