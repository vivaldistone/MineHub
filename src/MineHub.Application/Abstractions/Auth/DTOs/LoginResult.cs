namespace MineHub.Application.Abstractions.Auth.DTOs;

public record LoginResult(
    bool Success,
    string UserId,
    string JwtToken,
    string RefreshToken);
