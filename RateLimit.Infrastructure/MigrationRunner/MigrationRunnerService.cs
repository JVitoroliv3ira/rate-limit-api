using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using RateLimit.Infrastructure.Migrations;

namespace RateLimit.Infrastructure.MigrationRunner;

public static class MigrationRunnerService
{
    public static void AddMigrationRunner(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(V001_CreateUsersTable).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
    
    public static void RunMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}