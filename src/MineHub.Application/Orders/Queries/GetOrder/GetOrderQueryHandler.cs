using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Orders.Queries.GetOrder;

public class GetOrderQueryHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentDomainUserService _currentDomainUserService;

    public GetOrderQueryHandler(IOrderRepository orderRepository, ICurrentDomainUserService currentDomainUserService)
    {
        _orderRepository = orderRepository;
        _currentDomainUserService = currentDomainUserService;
    }

    public async Task<GetOrderResponse> HandleAsync(Guid id)
    {
        var user = await _currentDomainUserService.GetRequiredAsync();

        var order = await _orderRepository.GetByUserIdAndOrderIdAsync(user.Id, id);

        if (order is null)
            throw new NotFoundException("Order not found", "order_not_found");

        return new GetOrderResponse(
            order.Id, 
            order.CreatedAtUtc, 
            order.Status, 
            order.TotalPrice, 
            
            order.OrderItems.Select(o =>
            new GetOrderItemResponse(
                o.ProductId, 
                o.Name, 
                o.Description, 
                o.UnitPrice, 
                o.Quantity, 
                o.TotalPrice))
                .ToList());
    }
}
