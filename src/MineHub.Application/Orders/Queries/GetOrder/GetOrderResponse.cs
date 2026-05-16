using MineHub.Domain.Enums;
using MineHub.Domain.ValueObjects;

namespace MineHub.Application.Orders.Queries.GetOrder;

public record GetOrderResponse(Guid OrderId, DateTime CreatedAtUtc, OrderStatus Status, decimal TotalPrice, IReadOnlyCollection<GetOrderItemResponse> orderItems);