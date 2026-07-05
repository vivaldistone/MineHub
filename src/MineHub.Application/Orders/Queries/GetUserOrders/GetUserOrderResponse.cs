using MineHub.Domain.Enums;

namespace MineHub.Application.Orders.Queries.GetOrders;

public record GetUserOrderResponse(Guid OrderId, DateTime CreatedAtUtc, OrderStatus Status, decimal TotalPrice, IReadOnlyCollection<GetUserOrderItemResponse> orderItems);
