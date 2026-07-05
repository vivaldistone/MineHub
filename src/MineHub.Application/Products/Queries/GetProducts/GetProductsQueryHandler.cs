using MineHub.Application.Abstractions.Persistence;

namespace MineHub.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IReadOnlyCollection<GetProductResponse>> HandleAsync(CancellationToken token)
    {
        var products = await _productRepository.GetAllAsync(token);

        return products.Select(p =>
        new GetProductResponse(
            p.Id,
            p.Name,
            p.Description,
            p.Price))
            .ToList()
            .AsReadOnly();
    }
}
