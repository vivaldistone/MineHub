namespace MineHub.Api.Contracts.Requests.Products;

public sealed record CreateProductRequest(string Name, string Description, decimal Price);
