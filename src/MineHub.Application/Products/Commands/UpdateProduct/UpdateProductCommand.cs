namespace MineHub.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(Guid ProductId, string Name, string Description, decimal Price);
