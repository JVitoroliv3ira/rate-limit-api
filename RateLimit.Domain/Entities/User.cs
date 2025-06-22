using RateLimit.Domain.Enums;

namespace RateLimit.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Plan Plan { get; set; } = Plan.Free;
    
    public ICollection<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();
}