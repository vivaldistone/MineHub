namespace MineHub.Application.Products.Queries.GetProduct;

public record GetProductResponse(Guid ProductId, string Name, string Description, decimal Price);
