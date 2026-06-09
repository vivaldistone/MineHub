using MineHub.Application.Abstractions.Cache;
using MineHub.Application.Abstractions.Carts;
using MineHub.Application.Abstractions.Users;

namespace MineHub.Application.Carts.Commands.ClearCart;

public class ClearCartCommandHandler
{
    private readonly ICartCacheService _cartCacheService;
    private readonly ICurrentDomainUserService _currentDomainUserService;

    public ClearCartCommandHandler(
        ICartCacheService cartCacheService, 
        ICurrentDomainUserService currentDomainUserService,
        ICartService cartService)
    {
        _cartCacheService = cartCacheService;
        _currentDomainUserService = currentDomainUserService;
    }

    public async Task HandleAsync()
    {
        var domainUser = await _currentDomainUserService.GetRequiredAsync();

        await _cartCacheService.RemoveAsync(domainUser.Id);
    }
}
