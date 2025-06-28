using System.Security.Cryptography;
using System.Text;
using RateLimit.Application.Interfaces.Services;

namespace RateLimit.Infrastructure.Services;

public class SignatureService : ISignatureService
{
    public (string token, string hash) Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        var token = Convert.ToHexString(bytes).ToLower();
        var hash = Hash(token);
        return (token, hash);
    }

    public string Hash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLower();
    }
}