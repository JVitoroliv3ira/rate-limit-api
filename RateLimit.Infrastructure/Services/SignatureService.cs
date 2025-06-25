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

    public bool Validate(string token, string hash)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(hash))
            return false;

        var tokenHash = Hash(token);

        var a = Encoding.ASCII.GetBytes(tokenHash);
        var b = Encoding.ASCII.GetBytes(hash.ToLower());

        return CryptographicOperations.FixedTimeEquals(a, b);
    }

    private static string Hash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLower();
    }
}