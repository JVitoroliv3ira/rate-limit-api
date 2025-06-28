
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RateLimit.Api.Middleware;
using RateLimit.Api.Services.AuthenticatedUser;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Application.Interfaces.UseCases;
using RateLimit.Application.Services;
using RateLimit.Application.UseCases.ApiKeys;
using RateLimit.Application.UseCases.Auth;
using RateLimit.Application.UseCases.Users;
using RateLimit.Domain.Interfaces;
using RateLimit.Infrastructure.MigrationRunner;
using RateLimit.Infrastructure.Persistence;
using RateLimit.Infrastructure.Repositories;
using RateLimit.Infrastructure.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new Exception("Connection string not found");

builder.Services.AddMigrationRunner(connectionString);
builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
    };
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtService>();
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddScoped<ISignatureService, SignatureService>();

builder.Services.AddScoped<IRequestLimiterService, RequestLimiterService>();
builder.Services.AddScoped<IRateLimitPolicyProvider, StaticRateLimitPolicyProvider>();
builder.Services.AddScoped<IRateLimitStoreService, RedisRateLimitStoreService>();

builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
builder.Services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
builder.Services.AddScoped<ICreateApiKeyUseCase, CreateApiKeyUseCase>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RateLimitMiddleware>();

app.MapControllers();

app.Services.RunMigrations();

app.Run();