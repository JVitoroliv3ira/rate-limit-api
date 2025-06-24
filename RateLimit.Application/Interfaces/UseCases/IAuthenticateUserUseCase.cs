using RateLimit.Application.Common;
using RateLimit.Application.Dtos.Commands.Auth;
using RateLimit.Application.Dtos.Results.Auth;

namespace RateLimit.Application.Interfaces.UseCases;

public interface IAuthenticateUserUseCase
{
    Task<Either<ApiError, AuthenticationResult>> ExecuteAsync(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken
    );
}