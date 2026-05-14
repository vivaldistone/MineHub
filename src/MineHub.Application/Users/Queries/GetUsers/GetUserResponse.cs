namespace MineHub.Application.Users.Queries.GetUsers;

public record GetUserResponse(Guid Id, string Email, DateTime CreatedAtUtc, Guid? MinecraftUuid, string? MinecraftName);