namespace RateLimit.Domain.Entities;

public class ApiKey
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}