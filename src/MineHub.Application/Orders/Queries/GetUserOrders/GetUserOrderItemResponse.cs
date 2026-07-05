namespace MineHub.Application.Orders.Queries.GetOrders;

public record GetUserOrderItemResponse(Guid ProductId, string Name, string Description, decimal UnitPrice, int Quantity, decimal TotalPrice);
