namespace RateLimit.Application.Interfaces.Services;

public interface ISignatureService
{
    (string token, string hash) Generate();
    bool Validate(string token, string hash);
}