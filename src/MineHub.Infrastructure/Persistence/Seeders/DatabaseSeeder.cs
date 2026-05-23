namespace MineHub.Infrastructure.Persistence.Seeders;

public class DatabaseSeeder
{
    private readonly IdentitySeeder _identitySeeder;
    private readonly RoleSeeder _roleSeeder;
    private readonly ProductSeeder _productSeeder;

    public DatabaseSeeder(IdentitySeeder identitySeeder, RoleSeeder roleSeeder, ProductSeeder productSeeder)
    {
        _identitySeeder = identitySeeder;
        _roleSeeder = roleSeeder;
        _productSeeder = productSeeder;
    }

    public async Task SeedAsync()
    {
        await _identitySeeder.SeedAsync();
        await _roleSeeder.SeedAsync();
        await _productSeeder.SeedAsync();
    }
}
