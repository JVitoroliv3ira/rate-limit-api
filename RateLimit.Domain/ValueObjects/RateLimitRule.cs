namespace RateLimit.Domain.ValueObjects;

public record RateLimitRule
{
    public int Limit { get; init; }
    public TimeSpan Window { get; init; }
}