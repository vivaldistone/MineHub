using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;

namespace MineHub.Application.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly ICurrentDomainUserService _currentDomainUserService;
    private readonly IProductRepository _productRepository;

    public AddItemToCartCommandHandler(ICartRepository cartRepository, ICurrentDomainUserService currentDomainUserService, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _currentDomainUserService = currentDomainUserService;
        _productRepository = productRepository;
    }

    public async Task HandleAsync(AddItemToCartCommand command)
    {
        var domainUser = await _currentDomainUserService.GetRequiredAsync();

        var cart = await _cartRepository.GetByUserIdAsync(domainUser.Id);

        if (cart is null)
        {
            cart = new Cart(domainUser.Id);

            await _cartRepository.AddAsync(cart);
        }

        var product = await _productRepository.GetByIdAsync(command.ProductId);

        if (product is null)
            throw new NotFoundException("Product not found", "product_not_found");
        
        cart.AddItem(product, command.Quantity);
        await _cartRepository.UpdateAsync(cart);
    }
}
