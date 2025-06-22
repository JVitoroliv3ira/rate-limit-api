using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Infrastructure.Persistence;

namespace RateLimit.Infrastructure.Repositories;

public class Repository<T>(
    AppDbContext dbContext
) : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task<T?> GetByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IList<T>> ListByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task DeleteByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        var entities = await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        _dbSet.RemoveRange(entities);
    }

    public async Task<bool> ExistByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public IQueryable<T> Queryable => _dbSet.AsQueryable();
}