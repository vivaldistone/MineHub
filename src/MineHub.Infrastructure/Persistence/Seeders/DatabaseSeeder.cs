namespace MineHub.Infrastructure.Persistence.Seeders;

public class DatabaseSeeder
{
    private readonly IdentitySeeder _identitySeeder;
    private readonly RoleSeeder _roleSeeder;

    public DatabaseSeeder(IdentitySeeder identitySeeder, RoleSeeder roleSeeder)
    {
        _identitySeeder = identitySeeder;
        _roleSeeder = roleSeeder;
    }

    public async Task SeedAsync()
    {
        await _identitySeeder.SeedAsync();
        await _roleSeeder.SeedAsync();
    }
}
