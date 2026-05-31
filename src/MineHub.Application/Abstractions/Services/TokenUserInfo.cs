namespace MineHub.Application.Abstractions.Services;

public record TokenUserInfo(string authUserId, string Email, IReadOnlyCollection<string> Roles);
