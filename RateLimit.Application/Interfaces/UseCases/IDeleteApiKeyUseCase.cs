using RateLimit.Application.Common;

namespace RateLimit.Application.Interfaces.UseCases;

public interface IDeleteApiKeyUseCase
{
    Task<Either<ApiError, bool>> ExecuteAsync(
        int apiKeyId,
        int userId,
        CancellationToken cancellationToken
    );
}