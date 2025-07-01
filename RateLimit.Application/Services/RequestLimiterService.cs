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
    IApiLogger<RequestLimiterService> logger
) : IRequestLimiterService
{
    public async Task<bool> IsRequestAllowedAsync(string key)
    {
        var hash = signatureService.Hash(key);
        var apiKey = await apiKeyRepository.GetByAsync(k => k.Key == hash, CancellationToken.None, k => k.User);
        if (apiKey is null)
        {
            logger.LogWarning("Invalid API key attempt. Hash={Hash}", hash);
            throw new UnauthorizedAccessException("Invalid API key");
        }
        
        var plan = apiKey.User.Plan;
        var rule = policyProvider.GetRuleForPlan(plan);

        var allowed = await rateLimitStoreService.TryConsumeAsync(hash, rule);

        if (!allowed)
        {
            logger.LogInformation("Rate limit exceeded. KeyHash={Hash}, Plan={Plan}", hash, plan);
        }
        
        return allowed;
    }
}