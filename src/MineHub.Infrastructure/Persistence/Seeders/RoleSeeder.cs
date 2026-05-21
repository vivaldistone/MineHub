using Microsoft.AspNetCore.Identity;

namespace MineHub.Infrastructure.Persistence.Seeders;

public class RoleSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public const string Admin = "Admin";
    public const string User = "User";
    
    public RoleSeeder(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    
    public async Task SeedAsync()
    {
        if (!await _roleManager.RoleExistsAsync(Admin))
            await _roleManager.CreateAsync(new IdentityRole(Admin));

        if (!await _roleManager.RoleExistsAsync(User))
            await _roleManager.CreateAsync(new IdentityRole(User));
    }
}
