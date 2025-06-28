using RateLimit.Api.Attributes;
using RateLimit.Application.Interfaces.Services;

namespace RateLimit.Api.Middleware;

public class RateLimitMiddleware(
    RequestDelegate next    
)
{
    public async Task Invoke(HttpContext context, IRequestLimiterService limiter)
    {
        var endpoint = context.GetEndpoint();
        
        var hasAttribute = endpoint?.Metadata?.GetMetadata<RateLimitAttribute>() != null;
        if (!hasAttribute)
        {
            await next(context);
            return;
        }
        
        if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API key is missing.");
            return;
        }
        
        var isAllowed = await limiter.IsRequestAllowedAsync(apiKey!);
        
        if (!isAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Rate limit exceeded.");
            return;
        }

        await next(context);
    }
}