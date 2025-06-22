
using Microsoft.EntityFrameworkCore;
using RateLimit.Infrastructure.MigrationRunner;
using RateLimit.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new Exception("Connection string not found");

builder.Services.AddMigrationRunner(connectionString);
builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Services.RunMigrations();

app.Run();