using Microsoft.AspNetCore.Identity;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;
using MineHub.Application.Exceptions;
using MineHub.Application.Abstractions.Auth;
using MineHub.Application.Abstractions.Email;
using MineHub.Infrastructure.Auth.Entities;
using MineHub.Application.Auth.DTOs;

namespace MineHub.Infrastructure.Auth.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly IEmailSender _emailSender;

    public AccountService(UserManager<AuthUser> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IJwtTokenGenerator jwtTokenGenerator, 
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenHasher refreshTokenHasher,
        IEmailSender emailSender
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenHasher = refreshTokenHasher;
        _emailSender = emailSender;
    }

    public async Task<(bool Success, IEnumerable<string> Errors, string UserId)> CreateUserAsync(string email, string password, CancellationToken token)
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

    public async Task<TokenUserInfo> GetTokenUserInfoAsync(string authUserId, CancellationToken token)
    {
        var user = await  _userManager.FindByIdAsync(authUserId);

        if (user is null)
            throw new UnauthorizedException("user not authorize", "user_not_authorize");

        var roles = await _userManager.GetRolesAsync(user);

        return new TokenUserInfo(user.Id, user.Email!, roles.ToList());
    }

    public async Task<LoginResult> LoginAsync(string email, string password, CancellationToken token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return new LoginResult(false, null, null, null);

        var resultCheckPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!resultCheckPassword)
            return new LoginResult(false, null, null, null);

        var roles = await _userManager.GetRolesAsync(user);

        var jwtToken = _jwtGenerator.GenerateToken(user.Id, user.Email!, roles);

        var oldToken = await _refreshTokenRepository
            .GetTokenByUserAsync(user.Id, token);

        if (oldToken is not null)
        {
            await _refreshTokenRepository.DeleteAsync(oldToken, token);
        }    

        var refreshToken = _refreshTokenGenerator.Generate();

        var refreshTokenEntity = new RefreshToken(user.Id, _refreshTokenHasher.Hash(refreshToken), DateTime.UtcNow.AddDays(7));

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, token);

        return new LoginResult(true, user.Id, jwtToken, refreshToken);
    }

    public async Task RequestPasswordResetAsync(string email, CancellationToken token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new NotFoundException("User not found", "user_not_found");
        
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var resetLink = $"https://impish-irritant-shadiness.ngrok-free.dev/reset-password?token={resetToken}&email={email}";

        await _emailSender.SendEmailAsync
            (email, "Сброс пароля", resetLink, token);
    }

    public async Task ResetPasswordAsync(string email, string tokenReset, string newPassword, CancellationToken token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new NotFoundException("User not found", "user_not_found");

        var result = await _userManager.ResetPasswordAsync(user, tokenReset, newPassword);

        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.Select(s => s.Description).First());
        }
    }
}
