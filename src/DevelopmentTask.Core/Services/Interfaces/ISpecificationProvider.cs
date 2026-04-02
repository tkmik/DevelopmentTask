using System.Linq.Expressions;

namespace DevelopmentTask.Core.Services.Interfaces
{
    public interface ISpecificationProvider<TModel, TEntity>
    {
        Expression<Func<TEntity, bool>> GetFilter(TModel model);
    }
}
