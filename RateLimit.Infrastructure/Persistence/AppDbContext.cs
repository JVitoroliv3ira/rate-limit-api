using Microsoft.EntityFrameworkCore;
using RateLimit.Domain.Entities;

namespace RateLimit.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ApiKey> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("TB_USERS");
            e.HasKey(u => u.Id);

            e.Property(u => u.Id).HasColumnName("ID");
            e.Property(u => u.Email).HasColumnName("EMAIL");
            e.Property(u => u.Password).HasColumnName("PASSWORD");
            e.Property(u => u.Plan).HasColumnName("PLAN");
        });

        modelBuilder.Entity<ApiKey>(e =>
        {
            e.ToTable("TB_API_KEYS");
            e.HasKey(k => k.Id);
            
            e.Property(k => k.Id).HasColumnName("ID");
            e.Property(k => k.Name).HasColumnName("NAME");
            e.Property(k => k.Key).HasColumnName("KEY");
            e.Property(k => k.IsActive).HasColumnName("IS_ACTIVE");
            e.Property(k => k.CreatedAt).HasColumnName("CREATED_AT");
            e.Property(k => k.UserId).HasColumnName("USER_ID");
            
            e.HasOne(k => k.User)
                .WithMany(u => u.ApiKeys)
                .HasForeignKey(k => k.UserId);
        });
    }
}