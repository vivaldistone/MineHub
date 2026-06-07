namespace MineHub.Application.Abstractions.Auth.DTOs;

public record TokenUserInfo(string authUserId, string Email, IReadOnlyCollection<string> Roles);
