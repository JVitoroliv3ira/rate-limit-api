using RateLimit.Application.Common;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.UseCases;
using RateLimit.Domain.Entities;
using RateLimit.Domain.Enums;

namespace RateLimit.Application.UseCases.Users;

public class ChangePlanUseCase(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork
) : IChangePlanUseCase
{
    public async Task<Either<ApiError, bool>> ExecuteAsync(Plan plan, int userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return ApiError.Validation("User not found");
        }
        
        user.Plan = plan;

        await userRepository.UpdateAsync(user);
        await unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}