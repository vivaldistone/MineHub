using MineHub.Infrastructure.Persistence.Seeders;

namespace MineHub.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var seeder = scope.ServiceProvider
            .GetRequiredService<DatabaseSeeder>();

        await seeder.SeedAsync();
    }
}
