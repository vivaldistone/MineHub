using MineHub.Application.Abstractions.Cache;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;
using System.Text.Json;

namespace MineHub.Application.Products.Queries.GetProduct;

public class GetProductQueryHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cache;

    public GetProductQueryHandler(IProductRepository productRepository, ICacheService cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }


    public async Task<GetProductResponse> HandleAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Product id is required", nameof(id));

        var cacheKey = $"product:{id}";
        
        var productCache = await _cache.GetStringAsync(cacheKey);

        if (productCache is not null)
        {
            return JsonSerializer.Deserialize<GetProductResponse>(productCache)!;
        }
        
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException("Product was not found", "product_not_found");

        var response = new GetProductResponse(
            product.ProductId,
            product.Name,
            product.Description,
            product.Price);

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(product), TimeSpan.FromMinutes(10));

        return response;
    }
}
