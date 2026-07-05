namespace MineHub.Application.Auth.DTOs;

public record TokenUserInfo(string authUserId, string Email, IReadOnlyCollection<string> Roles);
