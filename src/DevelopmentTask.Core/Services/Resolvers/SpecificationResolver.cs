using System.Linq.Expressions;
using System.Reflection;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Core.Services.Resolvers
{
    internal sealed class SpecificationResolver(IServiceProvider provider) : ISpecificationResolver
    {
        private readonly IServiceProvider _provider = provider;

        public Expression<Func<TEntity, bool>> GetFilter<TEntity>(object query)
        {
            var queryType = query.GetType();
            var providerType = typeof(ISpecificationProvider<,>).MakeGenericType(queryType, typeof(TEntity));
            var provider = _provider.GetService(providerType);

            if (provider is null)
                return _ => true;

            var exec = GetSpecDelegate<TEntity>(queryType);

            return exec(provider, query);
        }

        private delegate Expression<Func<TEntity, bool>> SpecDelegate<TEntity>(object provider, object query);

        private static readonly MethodInfo ExecuteSpecMethodInfo
            = typeof(SpecificationResolver)
                .GetMethod(nameof(ExecuteSpecMethod), BindingFlags.Static | BindingFlags.NonPublic)!;
        private static Expression<Func<TEntity, bool>> ExecuteSpecMethod<TQuery, TEntity>(object provider, object query)
               => ((ISpecificationProvider<TQuery, TEntity>)provider).GetFilter((TQuery)query);
        private static SpecDelegate<TEntity> GetSpecDelegate<TEntity>(Type queryType)
        {
            var method = ExecuteSpecMethodInfo.MakeGenericMethod(queryType, typeof(TEntity));
            return (SpecDelegate<TEntity>)Delegate.CreateDelegate(typeof(SpecDelegate<TEntity>), method);
        }
    }
}
