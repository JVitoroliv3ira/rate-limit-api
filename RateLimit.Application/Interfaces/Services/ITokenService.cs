using RateLimit.Domain.Enums;

namespace RateLimit.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(int userId, string email, Plan plan);
}