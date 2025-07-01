using RateLimit.Application.Common;
using RateLimit.Domain.Enums;

namespace RateLimit.Application.Interfaces.UseCases;

public interface IChangePlanUseCase
{
    Task<Either<ApiError, bool>> ExecuteAsync(Plan plan, int userId, CancellationToken cancellationToken);
}