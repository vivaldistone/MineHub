namespace MineHub.Application.Users.Commands;

public record GetUserResponse(Guid Id, string Email, DateTime CreatedAtUtc, Guid? MinecraftUuid, string? MinecraftName);