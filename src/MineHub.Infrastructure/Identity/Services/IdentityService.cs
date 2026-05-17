using Microsoft.AspNetCore.Identity;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenHasher _refreshTokenHasher;

    public IdentityService(UserManager<AuthUser> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IJwtTokenGenerator jwtTokenGenerator, 
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenHasher refreshTokenHasher
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenHasher = refreshTokenHasher;
    }

    public async Task<(bool Success, IEnumerable<string> Errors, string UserId)> CreateUserAsync(string email, string password)
    {
        var user = new AuthUser
        {
            Email = email,
            UserName = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description), string.Empty);

        var roleExists = await _roleManager.RoleExistsAsync("user");

        if (!roleExists)
        {
            await _userManager.DeleteAsync(user);
            return (false, ["Role 'user' does not exists"], string.Empty);
        }
        
        var addRoleResult = await _userManager.AddToRoleAsync(user, "user");

        if (!addRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return (false, addRoleResult.Errors.Select(e => e.Description), string.Empty);
        }
        return (true, new List<string>(), user.Id);
    }

    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return new LoginResult(false, null, null, null);

        var resultCheckPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!resultCheckPassword)
            return new LoginResult(false, null, null, null);

        var roles = await _userManager.GetRolesAsync(user);

        var jwtToken = _jwtGenerator.GenerateToken(user.Id, user.Email!, roles);
        var refreshToken = _refreshTokenGenerator.Generate();

        var refreshTokenEntity = new RefreshToken(user.Id, _refreshTokenHasher.Hash(refreshToken), DateTime.UtcNow.AddDays(7));

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        return new LoginResult(true, user.Id, jwtToken, refreshToken);
    }
}
