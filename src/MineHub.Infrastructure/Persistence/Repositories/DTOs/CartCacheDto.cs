namespace MineHub.Infrastructure.Persistence.Repositories.DTOs;

public sealed record CartCacheDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public List<CartItemCacheDto> СartItems { get; init; } = new List<CartItemCacheDto>();
}
