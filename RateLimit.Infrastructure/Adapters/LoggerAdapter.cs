using Microsoft.Extensions.Logging;
using RateLimit.Application.Interfaces.Core;

namespace RateLimit.Infrastructure.Adapters;

public class LoggerAdapter<T> : IApiLogger<T>
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogWarning(string message, params object[] args) =>
        _logger.LogWarning(message, args);

    public void LogInformation(string message, params object[] args) =>
        _logger.LogInformation(message, args);
}