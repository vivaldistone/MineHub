namespace MineHub.Application.Carts.Queries;

public record GetCartItemResponse(Guid Id, string Name, string Description, decimal UnitPrice, int Quantity, decimal TotalPrice);
