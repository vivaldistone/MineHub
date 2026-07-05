using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Orders.Queries.GetUserOrder;

public class GetUserOrderQueryHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDomainUserResolver _currentDomainUserService;

    public GetUserOrderQueryHandler(IOrderRepository orderRepository, IDomainUserResolver currentDomainUserService)
    {
        _orderRepository = orderRepository;
        _currentDomainUserService = currentDomainUserService;
    }

    public async Task<GetUserOrderResponse> HandleAsync(Guid id, CancellationToken token)
    {
        var user = await _currentDomainUserService.GetRequiredAsync(token);

        var order = await _orderRepository.GetByUserIdAndOrderIdAsync(user.Id, id, token);

        if (order is null)
            throw new NotFoundException("Order not found", "order_not_found");

        return new GetUserOrderResponse(
            order.Id, 
            order.CreatedAtUtc, 
            order.Status, 
            order.TotalPrice, 
            
            order.OrderItems.Select(o =>
            new GetUserOrderItemResponse(
                o.ProductId, 
                o.Name, 
                o.Description, 
                o.UnitPrice, 
                o.Quantity, 
                o.TotalPrice))
                .ToList());
    }
}
