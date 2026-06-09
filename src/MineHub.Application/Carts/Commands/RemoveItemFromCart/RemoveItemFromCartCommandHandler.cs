using MineHub.Application.Abstractions.Cache;
using MineHub.Application.Abstractions.Carts;
using MineHub.Application.Abstractions.Users;

namespace MineHub.Application.Carts.Commands.RemoveItemFromCart;

public class RemoveItemFromCartCommandHandler
{
    private readonly ICurrentDomainUserService _currentDomainUserService;
    private readonly ICartCacheService _cartCacheService;
    private readonly ICartService _cartService;

    public RemoveItemFromCartCommandHandler(
        ICurrentDomainUserService currentDomainUserService,
        ICartCacheService cartCacheService,
        ICartService cartService)
    {
        _currentDomainUserService = currentDomainUserService;
        _cartCacheService = cartCacheService;
        _cartService = cartService;
    }

    public async Task HandleAsync(Guid productId)
    {
        var domainUser = await _currentDomainUserService.GetRequiredAsync();

        var cachedCart = await _cartCacheService.GetCartAsync(domainUser.Id);

        if (cachedCart is null)
            return;

        _cartService.RemoveItem(cachedCart, productId);

        if (cachedCart.CartItems.Count == 0)
        {
            await _cartCacheService.RemoveAsync(domainUser.Id);
            return;
        }

        await _cartCacheService.SetCartAsync(domainUser.Id, cachedCart, TimeSpan.FromDays(7));
    }
}
