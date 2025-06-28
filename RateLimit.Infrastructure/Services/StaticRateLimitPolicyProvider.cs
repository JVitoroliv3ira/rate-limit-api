using RateLimit.Domain.Enums;
using RateLimit.Domain.Interfaces;
using RateLimit.Domain.ValueObjects;

namespace RateLimit.Infrastructure.Services;

public class StaticRateLimitPolicyProvider : IRateLimitPolicyProvider
{
    public RateLimitRule GetRuleForPlan(Plan plan)
    {
        return plan switch
        {
            Plan.Free => new RateLimitRule { Limit = 100, Window = TimeSpan.FromHours(1)},
            Plan.Premium => new RateLimitRule { Limit = 1000, Window = TimeSpan.FromHours(1)},
            Plan.Enterprise => new RateLimitRule { Limit = 10000, Window = TimeSpan.FromHours(1)},
            _ => throw new ArgumentOutOfRangeException(nameof(plan), plan, null)
        };
    }
}