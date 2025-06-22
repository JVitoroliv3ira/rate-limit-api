using Microsoft.EntityFrameworkCore;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Infrastructure.Persistence;

namespace RateLimit.Infrastructure.Repositories;

public class Repository<T>(
    AppDbContext dbContext
) : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();
    
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public void Delete(T entity) => _dbSet.Remove(entity);

    public IQueryable<T> Queryable => _dbSet.AsQueryable();
}