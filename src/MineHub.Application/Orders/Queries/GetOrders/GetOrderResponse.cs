using MineHub.Domain.Enums;

namespace MineHub.Application.Orders.Queries.GetOrders;

public record GetOrderResponse(Guid OrderId, DateTime CreatedAtUtc, OrderStatus Status, decimal TotalPrice, IReadOnlyCollection<GetOrderItemResponse> orderItems);
