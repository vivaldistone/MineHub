using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Infrastructure.Identity;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Persistence.Seeders;

public class IdentitySeeder
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public IdentitySeeder(
        UserManager<AuthUser> userManager,
        IConfiguration configuration,
        IUserRepository userRepository)
    {
        _userManager = userManager;
        _configuration = configuration;
        _userRepository = userRepository;
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

        var authUser = await _userManager.FindByEmailAsync(email);
        
        if (authUser is null)
        {
            authUser = new AuthUser()
            {
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(authUser, password);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));

                throw new InvalidOperationException($"Failed to seed admin user {errors}");
            }
        }

        var domainUser = await _userRepository.GetByIdentityUserIdAsync(authUser.Id);

        if (domainUser is null)
        {
            domainUser = new User(authUser.Id, email);
            await _userRepository.AddAsync(domainUser);
        }    

    }
}
