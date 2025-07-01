using RateLimit.Application.Common;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.UseCases;
using RateLimit.Domain.Entities;

namespace RateLimit.Application.UseCases.ApiKeys;

public class DeleteApiKeyUseCase(
    IRepository<ApiKey> apiKeyRepository,
    IUnitOfWork unitOfWork
) : IDeleteApiKeyUseCase
{
    public async Task<Either<ApiError, bool>> ExecuteAsync(int apiKeyId, int userId, CancellationToken cancellationToken)
    {
        await apiKeyRepository.DeleteByAsync(
            k => k.Id == apiKeyId && k.UserId == userId,
            cancellationToken
        );
        
        await unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}