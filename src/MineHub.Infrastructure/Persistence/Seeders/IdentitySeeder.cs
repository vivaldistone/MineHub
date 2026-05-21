using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MineHub.Infrastructure.Identity;

namespace MineHub.Infrastructure.Persistence.Seeders;

public class IdentitySeeder
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IConfiguration _configuration;


    public IdentitySeeder(UserManager<AuthUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task SeedAsync()
    {
        var email = _configuration["Admin:Email"];
        var password = _configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("Admin email is not configured");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("Admin Password is not configured");
        }

        var user = new AuthUser()
        {
            Email = email,
            UserName = email
        };

        var existingUser = await _userManager.FindByEmailAsync(email);
        
        if (existingUser is not null)
            return;
   
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));

            throw new InvalidOperationException($"Failed to seed admin user {errors}");
        }
    }
}
