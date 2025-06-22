
using Microsoft.EntityFrameworkCore;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Application.Interfaces.UseCases;
using RateLimit.Application.UseCases.Users;
using RateLimit.Infrastructure.MigrationRunner;
using RateLimit.Infrastructure.Persistence;
using RateLimit.Infrastructure.Repositories;
using RateLimit.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new Exception("Connection string not found");

builder.Services.AddMigrationRunner(connectionString);
builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();

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