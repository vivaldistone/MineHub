using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler
{
    private readonly ICurrentDomainUserService _currentDomainUserService;
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(ICurrentDomainUserService currentDomainUserService, IOrderRepository orderRepository)
    {
        _currentDomainUserService = currentDomainUserService;
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyCollection<GetOrderResponse>> HandleAsync()
    {
        var user = await _currentDomainUserService.GetRequiredAsync();

        var orders = await _orderRepository.GetByUserIdAsync(user.Id);

        return orders.Select(o =>
            new GetOrderResponse(
                o.Id, 
                o.CreatedAtUtc, 
                o.Status, 
                o.TotalPrice,
                o.OrderItems.Select(i =>
                    new GetOrderItemResponse(
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