namespace RateLimit.Application.Interfaces.Core;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}