namespace MineHub.Application.Orders.Queries.GetOrders;

public sealed record GetOrderItemResponse(Guid ProductId, string Name, string Description, decimal UnitPrice, int Quantity, decimal TotalPrice);

