using MineHub.Domain.Exceptions;

namespace MineHub.Application.Abstractions.Cache.DTOs;

public class CartCacheDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public List<CartItemCacheDto> CartItems { get; set; } = [];

    public CartCacheDto(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId is required", "invalid_user_id");

        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }
}
