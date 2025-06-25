namespace RateLimit.Application.Interfaces.Services;

public interface IAuthenticatedUserService
{
    int? UserId { get; }
}