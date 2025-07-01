namespace RateLimit.Application.Interfaces.Core;

public interface IApiLogger<T>
{
    void LogWarning(string message, params object[] args);
    void LogInformation(string message, params object[] args);
}