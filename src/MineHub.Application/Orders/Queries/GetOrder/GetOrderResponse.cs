using MineHub.Domain.Enums;

namespace MineHub.Application.Orders.Queries.GetOrder;

public sealed record GetUserOrderResponse(Guid OrderId, DateTime CreatedAtUtc, OrderStatus Status, decimal TotalPrice, IReadOnlyCollection<GetUserOrderItemByIdResponse> orderItems);
