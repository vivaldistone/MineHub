namespace MineHub.Application.Orders.Queries.GetUserOrder;

public record GetUserOrderItemResponse(Guid ProductId, string Name, string Description, decimal UnitPrice, int Quantity, decimal TotalPrice);
