using MineHub.Application.Abstractions.Cache.DTOs;
using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Cache;

public interface ICartCacheService
{
    Task<CartCacheDto?> GetCartAsync(Guid userId);
    Task SetCartAsync(Guid userId, CartCacheDto cartDto, TimeSpan ttl);
    Task RemoveAsync(Guid userId);
}
