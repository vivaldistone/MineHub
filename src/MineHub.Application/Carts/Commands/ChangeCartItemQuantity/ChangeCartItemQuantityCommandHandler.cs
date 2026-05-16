using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Carts.Commands.ChangeCartItemQuantity;

public class ChangeCartItemQuantityCommandHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly ICurrentDomainUserService _currentDomainUserService;

    public ChangeCartItemQuantityCommandHandler(ICartRepository cartRepository, ICurrentDomainUserService currentDomainUserService)
    {
        _cartRepository = cartRepository;
        _currentDomainUserService = currentDomainUserService;
    }

    public async Task HandleAsync(ChangeCartItemQuantityCommand command)
    {
        var user = await _currentDomainUserService.GetRequiredAsync();

        var cart = await _cartRepository.GetByUserIdAsync(user.Id)
            ?? throw new NotFoundException("Cart not found", "cart_not_found");

        cart.ChangeQuantity(command.ProductId, command.Quantity);
        await _cartRepository.UpdateAsync(cart);
    }
}
