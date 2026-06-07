using MineHub.Application.Abstractions.Auth;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IIdentityService _identityService;

    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IJwtTokenGenerator jwtTokenGenerator, IIdentityService identityService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _identityService = identityService;
    }

    public async Task<string> HandleAsync(RefreshTokenCommand command)
    {
        var refreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(command.hash);
        
        if (refreshToken is null)
            throw new UnauthorizedException("user not authorize", "user_not_authorize");

        var authUserId = refreshToken.UserId;

        var userInfoToken = await _identityService.GetTokenUserInfoAsync(authUserId);

        var jwt = _jwtTokenGenerator.GenerateToken(
            userInfoToken.authUserId, 
            userInfoToken.Email, 
            userInfoToken.Roles);

        return jwt;
    }
}
