using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Products.Commands.AddProduct;

public class AddProductCommandHandler
{
    private readonly IProductRepository _productRepository;

    public AddProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<AddProductResponse> HandleAsync(AddProductCommand command)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));
        
        var productExist = await _productRepository.GetByNameAsync(command.Name.Trim());

        if (productExist is not null)
            throw new ConflictException("Product already exists", "product_already_exists");
        
        var product = new Product(command.Name, command.Description, command.Price);

        await _productRepository.AddAsync(product);

        return new AddProductResponse(
            product.ProductId);
    }
}
