using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;
using MineHub.Application.Orders.Queries.GetOrders;

namespace MineHub.Application.Orders.Queries.GetOrder;

public class GetOrderQueryHandler
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetUserOrderResponse> HandleAsync(Guid id, CancellationToken token)
    {
        var order = await _orderRepository.GetByIdAsync(id, token);

        if (order is null)
            throw new NotFoundException("Order not found", "order_not_found");

        return new GetUserOrderResponse(
            order.Id,
            order.CreatedAtUtc,
            order.Status,
            order.TotalPrice,
            order.OrderItems.Select(oi =>
            new GetUserOrderItemByIdResponse(
                oi.ProductId,
                oi.Name,
                oi.Description,
                oi.UnitPrice,
                oi.Quantity,
                oi.TotalPrice)).ToList());
    }
}
