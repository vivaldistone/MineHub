namespace MineHub.Application.Carts.Commands.AddItemToCart;

public record AddItemToCartCommand(Guid ProductId, int Quantity);