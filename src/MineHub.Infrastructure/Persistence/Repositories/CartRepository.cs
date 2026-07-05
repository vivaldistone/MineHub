using Microsoft.Extensions.Caching.Distributed;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;
using MineHub.Domain.ValueObjects;
using System.Text.Json;
using MineHub.Infrastructure.Persistence.Repositories.DTOs;

namespace MineHub.Infrastructure.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly IDistributedCache _distributedCache;
    private static readonly TimeSpan _timeSpan = TimeSpan.FromDays(7);

    public CartRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    private static string GetKey(Guid userId) => $"cart:{userId}";


    public async Task SaveAsync(Cart cart, CancellationToken token)
    {
        var cartCacheDto = new CartCacheDto
        {
            UserId = cart.UserId,
            CreatedAtUtc = cart.CreatedAtUtc,
            UpdatedAtUtc = cart.UpdatedAtUtc,
            СartItems = cart.CartItems.Select(ci =>
            new CartItemCacheDto()
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity
            }).ToList()
        };
        
        var json = JsonSerializer.Serialize(cartCacheDto);

        await _distributedCache.SetStringAsync(GetKey(cartCacheDto.UserId), json,
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = _timeSpan
            }, 
            token);
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken token)
    {
        var json = await _distributedCache.GetStringAsync(GetKey(userId), token);

        if (json is null)
            return null;

        var cartCacheDto = JsonSerializer.Deserialize<CartCacheDto>(json);

        if (cartCacheDto is null)
            return null;

        var cart = Cart.Rehydrate(
        cartCacheDto.UserId,
        cartCacheDto.СartItems.Select(x => new CartItem(x.ProductId, x.Quantity)).ToList(),
        cartCacheDto.CreatedAtUtc,
        cartCacheDto.UpdatedAtUtc
        );

        return cart;
    }

    public async Task RemoveAsync(Guid userId, CancellationToken token)
    {
        await _distributedCache.RemoveAsync(GetKey(userId), token);
    }
}
