using RateLimit.Application.Interfaces.Services;
using RateLimit.Domain.ValueObjects;
using StackExchange.Redis;

namespace RateLimit.Infrastructure.Services;

public class RedisRateLimitStoreService(
    IConnectionMultiplexer connection    
) : IRateLimitStoreService
{
    private readonly IDatabase _redis = connection.GetDatabase();
    
    public async Task<bool> TryConsumeAsync(string hashApiKey, RateLimitRule rule)
    {
        var key = $"ratelimit:{hashApiKey}";
        var ttl = (int)rule.Window.TotalSeconds;
        
        var current = await _redis.StringIncrementAsync(key);
        
        if (current == 1)
        {
            await _redis.KeyExpireAsync(key, TimeSpan.FromSeconds(ttl));
        }
        
        return current <= rule.Limit;
    }
}