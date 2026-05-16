namespace MineHub.Application.Carts.Commands.ChangeCartItemQuantity;

public record ChangeCartItemQuantityCommand(Guid ProductId, int Quantity);