namespace MineHub.Application.Abstractions.Services;

public record LoginResult(
    bool Success,
    string UserId,
    string JwtToken,
    string RefreshToken);
