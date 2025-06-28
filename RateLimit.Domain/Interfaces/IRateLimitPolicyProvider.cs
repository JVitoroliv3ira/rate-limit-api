using RateLimit.Domain.Enums;
using RateLimit.Domain.ValueObjects;

namespace RateLimit.Domain.Interfaces;

public interface IRateLimitPolicyProvider
{
    RateLimitRule GetRuleForPlan(Plan plan);
}