using MineHub.Application.Abstractions.Persistence;

namespace MineHub.Application.Orders.Queries.GetUserOrdersById;

public class GetUserOrdersByIdQueryHandler
{
    private readonly IOrderRepository _orderRepository;

    public GetUserOrdersByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<GetUserOrderByIdResponse>> HandleAsync(Guid userId, CancellationToken token)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId, token);

        return orders.Select(o =>
            new GetUserOrderByIdResponse(
                o.Id,
                o.CreatedAtUtc,
                o.Status,
                o.TotalPrice,
                o.OrderItems.Select(i =>
                    new GetUserOrderItemByIdResponse(
                        i.ProductId,
                        i.Name,
                        i.Description,
                        i.UnitPrice,
                        i.Quantity,
                        i.TotalPrice))
                    .ToList()))
                .ToList();
    }
}
