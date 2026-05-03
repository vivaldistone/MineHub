using MineHub.Domain.Entities;

namespace MineHub.Application.Products.Queries.GetProducts;

public record GetProductResponse(Guid ProductId, string Name, string Description, decimal Price);
