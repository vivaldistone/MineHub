using MineHub.Application.Exceptions;
using MineHub.Application.Abstractions.Cache;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Abstractions.Carts;

namespace MineHub.Application.Carts.Commands.ChangeCartItemQuantity;

public class ChangeCartItemQuantityCommandHandler
{
    private readonly ICurrentDomainUserService _currentDomainUserService;
    private readonly ICartCacheService _cartCacheService;
    private readonly ICartService _cartService;

    public ChangeCartItemQuantityCommandHandler(
        ICurrentDomainUserService currentDomainUserService,
        ICartCacheService cartCacheService,
        ICartService cartService)
    {
        _currentDomainUserService = currentDomainUserService;
        _cartCacheService = cartCacheService;
        _cartService = cartService;
    }

    public async Task HandleAsync(ChangeCartItemQuantityCommand command)
    {
        var user = await _currentDomainUserService.GetRequiredAsync();

        var cachedCart = await _cartCacheService.GetCartAsync(user.Id);

        if (cachedCart is null)
            throw new NotFoundException("Cart not found in cache", "cart_not_found_in_cache");

        _cartService.ChangeQuantity(cachedCart,command.ProductId,command.Quantity);

        await _cartCacheService.SetCartAsync(user.Id,cachedCart, TimeSpan.FromDays(7));
    }
}
