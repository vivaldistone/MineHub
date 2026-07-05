namespace MineHub.Application.Auth.DTOs;

public record LoginResult(
    bool Success,
    string UserId,
    string JwtToken,
    string RefreshToken);
