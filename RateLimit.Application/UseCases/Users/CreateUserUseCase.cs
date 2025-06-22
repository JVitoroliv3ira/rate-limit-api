using RateLimit.Application.Common;
using RateLimit.Application.Dtos.Commands.Users;
using RateLimit.Application.Dtos.Results.Users;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Application.Interfaces.UseCases;
using RateLimit.Domain.Entities;
using RateLimit.Domain.Enums;

namespace RateLimit.Application.UseCases.Users;

public class CreateUserUseCase(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher
) : ICreateUserUseCase
{
    public async Task<Either<ApiError, CreateUserResult>> ExecuteAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var emailInUse = await userRepository.ExistByAsync(
            u => u.Email.ToLower() == command.Email.ToLower(),
            cancellationToken
        );

        if (emailInUse)
        {
            return ApiError.Validation("Email is already in use");
        }

        var user = new User
        {
            Email = command.Email,
            Password = passwordHasher.Hash(command.Password),
            Plan = Plan.Free
        };

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateUserResult(user.Id, user.Email);
    }
}