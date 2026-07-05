using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(UpdateProductCommand command, CancellationToken token)
    {
        if (command is null) 
            throw new ArgumentNullException(nameof(command));

        var product = await _productRepository.GetByIdAsync(command.ProductId, token);

        if (product is null)
            throw new NotFoundException("Product was not found", "not_found_product");

        if (await _productRepository.GetByNameAsync(command.Name.Trim(), token) is not null && command.ProductId != product.Id)
            throw new ConflictException("Product Name exists", "product_name_exists");

        product.ChangeName(command.Name);
        product.ChangeDescription(command.Description);
        product.ChangePrice(command.Price);

        await _unitOfWork.SaveChangesAsync(token);
    }
}
