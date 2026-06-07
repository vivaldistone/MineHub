using MineHub.Application.Abstractions.Cache;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Abstractions.Cache.DTOs;

namespace MineHub.Application.Carts.Queries.GetCart;

public class GetCartQueryHandler
{
    private readonly ICurrentDomainUserService _currentDomainUserService;
    private readonly ICartCacheService _cartCacheService;

    public GetCartQueryHandler(ICurrentDomainUserService currentDomainUserService, ICartCacheService cartCacheService)
    {
        _currentDomainUserService = currentDomainUserService;
        _cartCacheService = cartCacheService;
    }

    public async Task<GetCartResponse> HandleAsync()
    {     
        var domainUser = await _currentDomainUserService.GetRequiredAsync();

        var cart = await _cartCacheService.GetCartAsync(domainUser.Id);

        if (cart is null)
        {
            cart = new CartCacheDto(domainUser.Id);

            await _cartCacheService.SetCartAsync(domainUser.Id, cart, TimeSpan.FromDays(7));
        }

        return new GetCartResponse(
            cart.Id,
            cart.CreatedAtUtc,
            cart.UpdatedAtUtc,
            cart.CartItems.Sum(c => c.TotalPrice),

            cart.CartItems.Select(c => new GetCartItemResponse(
            c.ProductId,
            c.ProductName,
            c.Description,
            c.UnitPrice,
            c.Quantity,
            c.TotalPrice)).ToList()
            );
    }
}
