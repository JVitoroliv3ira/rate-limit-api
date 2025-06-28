using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Domain.Entities;
using RateLimit.Domain.Interfaces;

namespace RateLimit.Application.Services;

public class RequestLimiterService(
    IRateLimitPolicyProvider policyProvider,
    IRepository<ApiKey> apiKeyRepository,
    IRateLimitStoreService rateLimitStoreService,
    ISignatureService signatureService
) : IRequestLimiterService
{
    public async Task<bool> IsRequestAllowedAsync(string key)
    {
        var hash = signatureService.Hash(key);
        var apiKey = await apiKeyRepository.GetByAsync(k => k.Key == hash, CancellationToken.None, k => k.User);
        if (apiKey is null)
        {
            throw new UnauthorizedAccessException("Invalid API key");
        }
        
        var plan = apiKey.User.Plan;
        var rule = policyProvider.GetRuleForPlan(plan);

        return await rateLimitStoreService.TryConsumeAsync(hash, rule);
    }
}