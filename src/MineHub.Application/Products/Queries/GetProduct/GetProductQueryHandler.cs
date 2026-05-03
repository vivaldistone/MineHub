using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Products.Queries.GetProduct;

public class GetProductQueryHandler
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    public async Task<GetProductResponse> HandleAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Product id is required", nameof(id));
        
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException("Product was not found", "product_not_found");

        return new GetProductResponse(
            product.ProductId,
            product.Name,
            product.Description,
            product.Price);
    }
}
