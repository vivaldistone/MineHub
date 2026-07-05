namespace MineHub.Application.Orders.Queries.GetUserOrdersById;

public sealed record GetUserOrderItemByIdResponse(Guid ProductId, string Name, string Description, decimal UnitPrice, int Quantity, decimal TotalPrice);

