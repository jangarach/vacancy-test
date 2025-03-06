using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Payment.Infrastructure.Database;

namespace Payment.Api.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var databaseCreator = dbContext.Database.GetService<IRelationalDatabaseCreator>();

        if (!await databaseCreator.ExistsAsync(cancellationToken))
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any())
            {
                app.Logger.LogInformation("Applying migrations to database...");
                await dbContext.Database.MigrateAsync(cancellationToken);
                app.Logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                app.Logger.LogInformation("No pending migrations found.");
            }
        }
    }
}