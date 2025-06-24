using RateLimit.Application.Common;
using RateLimit.Application.Dtos.Commands.Auth;
using RateLimit.Application.Dtos.Results.Auth;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Application.Interfaces.UseCases;
using RateLimit.Domain.Entities;

namespace RateLimit.Application.UseCases.Auth;

public class AuthenticateUserUseCase(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService
) : IAuthenticateUserUseCase
{
    public async Task<Either<ApiError, AuthenticationResult>> ExecuteAsync(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository
            .GetByAsync(u => u.Email.ToLower() == command.Email.ToLower(),
                cancellationToken
            );

        if (user is null || !passwordHasher.Verify(command.Password, user.Password))
        {
            return ApiError.Validation("Invalid email or password");
        }
        
        var accessToken = tokenService.GenerateAccessToken(
            user.Id,
            user.Email,
            user.Plan
        );

        return new AuthenticationResult(
            AccessToken: accessToken,
            UserId: user.Id
        );
    }
}