using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;

namespace MineHub.Application.Orders.Commands.CreateOrderFromCart;

public class CreateOrderFromCartCommandHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentDomainUserService _currentDomainUserService;

    public CreateOrderFromCartCommandHandler(ICartRepository cartRepository, IOrderRepository orderRepository, ICurrentDomainUserService currentDomainUserService)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _currentDomainUserService = currentDomainUserService;
    }

    public async Task HandleAsync()
    {
        var user = await _currentDomainUserService.GetRequiredAsync();

        var createOrder = await _orderRepository.GetCreatedByUserIdAsync(user.Id);

        if (createOrder is not null)
        {
            throw new ConflictException("Order already exists", "order_already_exists");
        }

        var cart = await _cartRepository.GetByUserIdAsync(user.Id)
            ?? throw new NotFoundException("Cart not found", "cart_not_found");

        var order = Order.Create(cart);
        cart.Clear();

        await _orderRepository.AddAsync(order);
        await _cartRepository.UpdateAsync(cart);
    }
}
