using System.Linq.Expressions;

namespace DevelopmentTask.Core.Services.Interfaces
{
    public interface ISpecificationResolver
    {
        Expression<Func<TEntity, bool>> GetFilter<TEntity>(object query);
    }
}
