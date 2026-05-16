namespace MineHub.Application.Orders.Queries.GetOrder;

public record GetOrderItemResponse(Guid ProductId, string Name, string Description, decimal UnitPrice, int Quantity, decimal TotalPrice);
