using RateLimit.Domain.ValueObjects;

namespace RateLimit.Application.Interfaces.Services;

public interface IRateLimitStoreService
{
    Task<bool> TryConsumeAsync(string hashApiKey, RateLimitRule rule);
}