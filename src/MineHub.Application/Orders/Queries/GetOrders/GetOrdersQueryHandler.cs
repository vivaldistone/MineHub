using MineHub.Application.Abstractions.Persistence;

namespace MineHub.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository= orderRepository;
    }

    public async Task<List<GetOrderResponse>> HandleAsync(CancellationToken token)
    {
        var orders = await _orderRepository.GetAllAsync(token);

        return orders.Select(o =>
        new GetOrderResponse(
            o.Id,
            o.CreatedAtUtc,
            o.Status,
            o.TotalPrice,
            o.OrderItems.Select(oi =>
            new GetOrderItemResponse(
                oi.ProductId,
                oi.Name,
                oi.Description,
                oi.UnitPrice,
                oi.Quantity,
                oi.TotalPrice)).ToList())).ToList();
    }
}
