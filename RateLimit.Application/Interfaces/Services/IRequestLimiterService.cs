namespace RateLimit.Application.Interfaces.Services;

public interface IRequestLimiterService
{
    Task<bool> IsRequestAllowedAsync(string apiKey);
}