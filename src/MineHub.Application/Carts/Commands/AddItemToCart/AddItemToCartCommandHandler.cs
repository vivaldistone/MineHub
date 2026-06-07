using MineHub.Application.Abstractions.Cache;
using MineHub.Application.Abstractions.Cache.DTOs;
using MineHub.Application.Abstractions.Carts;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandHandler
{
    private readonly ICurrentDomainUserService _currentDomainUserService;
    private readonly IProductRepository _productRepository;
    private readonly ICartCacheService _cartCacheService;
    private readonly ICartService _cartService;

    public AddItemToCartCommandHandler(
        ICurrentDomainUserService currentDomainUserService, 
        IProductRepository productRepository, 
        ICartCacheService cartCacheService,
        ICartService cartService)
    {
        _currentDomainUserService = currentDomainUserService;
        _productRepository = productRepository;
        _cartCacheService = cartCacheService;
        _cartService = cartService;
    }

    public async Task HandleAsync(AddItemToCartCommand command)
    {
        var domainUser = await _currentDomainUserService.GetRequiredAsync();

        var cachedCart = await _cartCacheService.GetCartAsync(domainUser.Id) ??
            new CartCacheDto(domainUser.Id);
        
        var product = await _productRepository.GetByIdAsync(command.ProductId);

        if (product is null)
            throw new NotFoundException("Product not found", "product_not_found");

        _cartService.AddItem(cachedCart, product, command.Quantity);
        
        await _cartCacheService.SetCartAsync(domainUser.Id, cachedCart, TimeSpan.FromDays(7));
    }
}
