using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(Guid id)
    {
        if (id == Guid.Empty) 
            throw new ArgumentException("Product id is required", nameof(id));

        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException("Product was not found", "product_not_found");

        await _productRepository.DeleteAsync(product);  
    }
}
