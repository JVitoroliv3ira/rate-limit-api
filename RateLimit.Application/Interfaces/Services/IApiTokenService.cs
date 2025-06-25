namespace RateLimit.Application.Interfaces.Services;

public interface IApiTokenService
{
    (string token, string hash) Generate();
    bool Validate(string token, string hash);
}