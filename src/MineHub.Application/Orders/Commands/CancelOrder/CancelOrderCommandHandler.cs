using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler
{
    private readonly ICurrentDomainUserService _currentDomainUserService;
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(ICurrentDomainUserService currentDomainUserService, IOrderRepository orderRepository)
    {
        _currentDomainUserService = currentDomainUserService;
        _orderRepository = orderRepository;
    }

    public async Task HandleAsync()
    {
        var user = await _currentDomainUserService.GetRequiredAsync();

        var createdOrder = await _orderRepository.GetCreatedByUserIdAsync(user.Id);

        if (createdOrder is null)
            throw new NotFoundException("Created order not found", "created_order_not_found");

        createdOrder.Cancel();
        await _orderRepository.UpdateAsync(createdOrder);
    }
}
