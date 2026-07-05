using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;

namespace MineHub.Application.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandHandler
{
    private readonly IDomainUserResolver _domainUserResolver;
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;

    public AddItemToCartCommandHandler(
        IDomainUserResolver domainUserResolver, 
        IProductRepository productRepository,
        ICartRepository cartRepository)
    {
        _domainUserResolver = domainUserResolver;
        _productRepository = productRepository;
        _cartRepository = cartRepository;
    }

    public async Task HandleAsync(AddItemToCartCommand command, CancellationToken token)
    {
        var domainUser = await _domainUserResolver.GetRequiredAsync(token);

        var cart = await _cartRepository.GetByUserIdAsync(domainUser.Id, token);

        if (cart is null)
            cart = new Cart(domainUser.Id);

        var product = await _productRepository.GetByIdAsync(command.ProductId, token);

        if (product is null)
            throw new NotFoundException("Product not found", "product_not_found");

        if (!product.IsActive)
            throw new BusinessRuleException("Product is not active", "product_not_active");

        //concurency fix in future
        cart.AddItem(product.Id, command.Quantity);
        await _cartRepository.SaveAsync(cart, token);
    }
}
