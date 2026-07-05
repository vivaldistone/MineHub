using MineHub.Application.Abstractions.Users;
using MineHub.Application.Abstractions.Persistence;

namespace MineHub.Application.Carts.Commands.ChangeCartItemQuantity;

public class ChangeCartItemQuantityCommandHandler
{
    private readonly IDomainUserResolver _domainUserResolver;
    private readonly ICartRepository _cartRepository;

    public ChangeCartItemQuantityCommandHandler(
        IDomainUserResolver domainUserResolver,
        ICartRepository cartRepository)
    {
        _domainUserResolver = domainUserResolver;
        _cartRepository = cartRepository;
    }

    public async Task HandleAsync(ChangeCartItemQuantityCommand command, CancellationToken token)
    {
        var domainUser = await _domainUserResolver.GetRequiredAsync(token);

        var cart = await _cartRepository.GetByUserIdAsync(domainUser.Id, token);

        if (cart is null)
            return;

        cart.ChangeQuantity(command.ProductId, command.Quantity);

        if (!cart.CartItems.Any())
            await _cartRepository.RemoveAsync(domainUser.Id, token);
        else
            await _cartRepository.SaveAsync(cart, token);
    }
}
