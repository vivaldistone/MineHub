namespace MineHub.Infrastructure.Persistence.Repositories.DTOs;

public sealed record CartItemCacheDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}
