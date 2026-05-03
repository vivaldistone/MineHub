namespace MineHub.Application.Products.Commands.AddProduct;

public record AddProductResponse(Guid ProductId, string Name, string Description, decimal Price);
