using RateLimit.Application.Common;
using RateLimit.Application.Dtos.Commands.ApiKeys;
using RateLimit.Application.Dtos.Results.ApiKeys;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Application.Interfaces.UseCases;
using RateLimit.Domain.Entities;

namespace RateLimit.Application.UseCases.ApiKeys;

public class CreateApiKeyUseCase(
    IRepository<User> userRepository,
    IRepository<ApiKey> apiKeyRepository,
    ISignatureService signatureService
) : ICreateApiKeyUseCase
{
    public async Task<Either<ApiError, CreateApiKeyResult>> ExecuteAsync(
        CreateApiKeyCommand command,
        int userId,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistByAsync(u => u.Id == userId, cancellationToken);
        if (!userExists)
        {
            return ApiError.Validation("User not found.");
        }

        var (apiKey, hashedApiKey) = signatureService.Generate();

        var key = new ApiKey
        {
            Name = command.Name,
            Key = hashedApiKey,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            UserId = userId
        };

        await apiKeyRepository.AddAsync(key, cancellationToken);

        var result = new CreateApiKeyResult(
            Name: key.Name,
            Key: apiKey
        );

        return result;
    }
}