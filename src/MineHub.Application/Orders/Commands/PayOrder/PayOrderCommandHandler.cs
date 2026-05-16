using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;

namespace MineHub.Application.Orders.Commands.PayOrder;

public class PayOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentDomainUserService _domainUserService;
    
    public PayOrderCommandHandler(IOrderRepository orderRepository, ICurrentDomainUserService domainUserService)
    {
        _orderRepository = orderRepository;
        _domainUserService = domainUserService;
    }

    public async Task HandleAsync()
    {
        var user = await _domainUserService.GetRequiredAsync();

        var createdOrder = await _orderRepository.GetCreatedByUserIdAsync(user.Id);

        if(createdOrder is null)
        {
            throw new NotFoundException("Created order not found", "created_order_not_found");
        }
            
        createdOrder.Pay();
        await _orderRepository.UpdateAsync(createdOrder);
    }

}
