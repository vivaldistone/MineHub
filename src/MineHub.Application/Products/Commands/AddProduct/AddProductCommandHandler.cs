using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Products.Commands.AddProduct;

public class AddProductCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddProductResponse> HandleAsync(AddProductCommand command, CancellationToken token)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));
        
        var productExist = await _productRepository.GetByNameAsync(command.Name.Trim(), token);

        if (productExist is not null)
            throw new ConflictException("Product already exists", "product_already_exists");
        
        var product = new Product(command.Name, command.Description, command.Price);

        await _productRepository.AddAsync(product, token);
        
        await _unitOfWork.SaveChangesAsync(token);

        return new AddProductResponse(
            product.Id);
    }
}
