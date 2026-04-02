using System.Linq.Expressions;

namespace DevelopmentTask.Core.Models.Specifications
{
    public abstract class Specification<TModel>
    {
        public abstract Expression<Func<TModel, bool>> ToExpression();
    }
}
