using MineHub.Domain.Enums;

namespace MineHub.Application.Orders.Queries.GetOrders;

public sealed record GetOrderResponse(Guid OrderId, DateTime CreatedAtUtc, OrderStatus Status, decimal TotalPrice, IReadOnlyCollection<GetOrderItemResponse> orderItems);
