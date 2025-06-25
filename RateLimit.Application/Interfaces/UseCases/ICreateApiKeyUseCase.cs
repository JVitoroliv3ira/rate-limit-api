using RateLimit.Application.Common;
using RateLimit.Application.Dtos.Commands.ApiKeys;
using RateLimit.Application.Dtos.Results.ApiKeys;

namespace RateLimit.Application.Interfaces.UseCases;

public interface ICreateApiKeyUseCase
{
    Task<Either<ApiError, CreateApiKeyResult>> ExecuteAsync(
        CreateApiKeyCommand command,
        int userId,
        CancellationToken cancellationToken
    );
}