using MineHub.Application.Abstractions.Cache;
using MineHub.Application.Abstractions.Cache.DTOs;
using System.Text.Json;

namespace MineHub.Infrastructure.Redis;

public class CartCacheService : ICartCacheService
{
    private readonly ICacheService _cacheService;

    public CartCacheService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<CartCacheDto?> GetCartAsync(Guid userId)
    {
        var cacheKey = $"cart:user:{userId}";

        var json = await _cacheService.GetStringAsync(cacheKey);

        if (json is null)
            return null;

        return JsonSerializer.Deserialize<CartCacheDto>(json);
    }

    public async Task RemoveAsync(Guid userId)
    {
        var cacheKey = $"cart:user:{userId}";
        await _cacheService.RemoveAsync(cacheKey);
    }

    public async Task SetCartAsync(Guid userId, CartCacheDto value, TimeSpan ttl)
    {
        var cacheKey = $"cart:user:{userId}";
        await _cacheService.SetStringAsync(cacheKey, JsonSerializer.Serialize<CartCacheDto>(value), ttl);
    }
}
