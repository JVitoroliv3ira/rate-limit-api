using RateLimit.Application.Common;
using RateLimit.Application.Dtos.Commands.Users;
using RateLimit.Application.Dtos.Results.Users;

namespace RateLimit.Application.Interfaces.UseCases;

public interface ICreateUserUseCase
{
    Task<Either<ApiError, CreateUserResult>> ExecuteAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken
    );
}