namespace RateLimit.Application.Interfaces.Core;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    void Delete(T entity);
    IQueryable<T> Queryable { get; }
}