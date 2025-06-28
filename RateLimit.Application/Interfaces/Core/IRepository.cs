using System.Linq.Expressions;

namespace RateLimit.Application.Interfaces.Core;

public interface IRepository<T> where T : class
{
    Task<T?> GetByAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes
    );

    Task<IList<T>> ListByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task DeleteByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> ExistByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity);
    IQueryable<T> Queryable { get; }
}