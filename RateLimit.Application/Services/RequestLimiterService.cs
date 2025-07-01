using Microsoft.Extensions.Caching.Memory;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Domain.Entities;
using RateLimit.Domain.Interfaces;

namespace RateLimit.Application.Services;

public class RequestLimiterService(
    IRateLimitPolicyProvider policyProvider,
    IRepository<ApiKey> apiKeyRepository,
    IRateLimitStoreService rateLimitStoreService,
    ISignatureService signatureService,
    IMemoryCache cache,
    IApiLogger<RequestLimiterService> logger
) : IRequestLimiterService
{
    private const string CachePrefix = "apikey::";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(1);

    public async Task<bool> IsRequestAllowedAsync(string key)
    {
        var hash = signatureService.Hash(key);
        
        var apiKey = await cache.GetOrCreateAsync(
            CachePrefix + hash,
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheTtl;
                
                return await apiKeyRepository.GetByAsync(
                    k => k.Key == hash,
                    CancellationToken.None,
                    k => k.User);
            });

        if (apiKey is null)
        {
            logger.LogWarning("Invalid API key attempt. Hash={Hash}", hash);
            throw new UnauthorizedAccessException("Invalid API key");
        }

        var rule = policyProvider.GetRuleForPlan(apiKey.User.Plan);

        var allowed = await rateLimitStoreService.TryConsumeAsync(hash, rule);

        if (!allowed)
        {
            logger.LogInformation(
                "Rate limit exceeded. KeyHash={Hash}, Plan={Plan}",
                hash, apiKey.User.Plan);
        }

        return allowed;
    }
}