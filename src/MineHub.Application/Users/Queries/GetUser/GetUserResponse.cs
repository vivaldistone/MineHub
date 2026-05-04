namespace MineHub.Application.Users.Queries.GetUser;

public record GetUserResponse(Guid Id, string Email, DateTime CreatedAtUtc, Guid? MinecraftUuid, string? MinecraftName);
