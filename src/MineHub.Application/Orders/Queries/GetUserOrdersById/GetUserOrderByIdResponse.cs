using MineHub.Domain.Enums;

namespace MineHub.Application.Orders.Queries.GetUserOrdersById;

public sealed record GetUserOrderByIdResponse(Guid OrderId, DateTime CreatedAtUtc, OrderStatus Status, decimal TotalPrice, IReadOnlyCollection<GetUserOrderItemByIdResponse> orderItems);
