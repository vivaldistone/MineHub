using Microsoft.AspNetCore.Mvc;
using MineHub.Api.Contracts.Requests.RefreshToken;
using MineHub.Application.Auth.Commands.RefreshToken;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class RefreshJwtTokenController : ControllerBase
{
    private readonly RefreshTokenCommandHandler _refreshTokenHandler;

    public RefreshJwtTokenController(RefreshTokenCommandHandler refreshTokenHandler)
    {
        _refreshTokenHandler = refreshTokenHandler;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> GetToken(RefreshTokenRequest request)
    {
        var refreshTokenCommand = new RefreshTokenCommand(request.Hash);

        var jwt = await _refreshTokenHandler.HandleAsync(refreshTokenCommand);

        return Ok(jwt);
    }
}
