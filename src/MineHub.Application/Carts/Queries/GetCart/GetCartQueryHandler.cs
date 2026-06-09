using MineHub.Application.Abstractions.Cache;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Abstractions.Cache.DTOs;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Carts;

namespace MineHub.Application.Carts.Queries.GetCart;

public class GetCartQueryHandler
{
    private readonly ICurrentDomainUserService _currentDomainUserService;
    private readonly ICartCacheService _cartCacheService;
    private readonly IProductRepository _productRepository;
    private readonly ICartService _cartService;

    public GetCartQueryHandler(
        ICurrentDomainUserService currentDomainUserService, 
        ICartCacheService cartCacheService,
        IProductRepository productRepository,
        ICartService cartService)
    {
        _currentDomainUserService = currentDomainUserService;
        _cartCacheService = cartCacheService;
        _productRepository = productRepository;
        _cartService = cartService;
    }

    public async Task<GetCartResponse> HandleAsync()
    {     
        var domainUser = await _currentDomainUserService.GetRequiredAsync();

        var cartCached = await _cartCacheService.GetCartAsync(domainUser.Id);

        if (cartCached is null)
        {
            cartCached = new CartCacheDto(domainUser.Id);

            await _cartCacheService.SetCartAsync(domainUser.Id, cartCached, TimeSpan.FromDays(7));
        }

        var productsIds = cartCached.CartItems.Select(c => c.ProductId).ToList();

        var products = await _productRepository.GetByIdsAsync(productsIds);

        var wasUpdatedCart = _cartService.RefreshItemsFromProducts(cartCached, products);
        
        if (wasUpdatedCart)
        {
            await _cartCacheService.SetCartAsync(domainUser.Id, cartCached, TimeSpan.FromDays(7));
        }

        return new GetCartResponse(
            cartCached.Id,
            cartCached.CreatedAtUtc,
            cartCached.UpdatedAtUtc,
            cartCached.CartItems.Sum(c => c.TotalPrice),

            cartCached.CartItems.Select(c => new GetCartItemResponse(
            c.ProductId,
            c.ProductName,
            c.Description,
            c.UnitPrice,
            c.Quantity,
            c.TotalPrice)).ToList()
            );
    }
}
