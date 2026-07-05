using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;
using System.Text.Json;

namespace MineHub.Application.Products.Queries.GetProduct;

public class GetProductQueryHandler
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductResponse> HandleAsync(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Product id is required", nameof(id));

        var product = await _productRepository.GetByIdAsync(id, token)
            ?? throw new NotFoundException("Product was not found", "product_not_found");

        var response = new GetProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price);

        return response;
    }
}
