namespace RateLimit.Application.Interfaces.Services;

public interface ISignatureService
{
    (string token, string hash) Generate();
    string Hash(string input);
}