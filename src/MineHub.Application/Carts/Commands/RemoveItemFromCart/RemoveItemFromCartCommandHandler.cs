using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;

namespace MineHub.Application.Carts.Commands.RemoveItemFromCart;

public class RemoveItemFromCartCommandHandler
{
    private readonly IDomainUserResolver _domainUserResolver;
    private readonly ICartRepository _cartRepository;

    public RemoveItemFromCartCommandHandler(
        IDomainUserResolver domainUserResolver,
        ICartRepository cartRepository)
    {
        _domainUserResolver = domainUserResolver;
        _cartRepository = cartRepository;
    }

    public async Task HandleAsync(Guid productId, CancellationToken token)
    {
        var domainUser = await _domainUserResolver.GetRequiredAsync(token);

        var cart = await _cartRepository.GetByUserIdAsync(domainUser.Id, token);

        if (cart is null)
            return;

        cart.RemoveItem(productId);

        if (!cart.CartItems.Any())
            await _cartRepository.RemoveAsync(domainUser.Id, token);
        else
            await _cartRepository.SaveAsync(cart, token);
    }
}
