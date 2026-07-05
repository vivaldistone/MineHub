using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(
        IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty) 
            throw new ArgumentException("Product id is required", nameof(id));

        var product = await _productRepository.GetByIdAsync(id, token);

        if (product is null)
            throw new NotFoundException("Product was not found", "product_not_found");

        await _productRepository.DeleteAsync(product, token);

        await _unitOfWork.SaveChangesAsync(token);
    }
}
