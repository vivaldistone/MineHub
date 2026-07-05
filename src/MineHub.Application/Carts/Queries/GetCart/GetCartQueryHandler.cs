using MineHub.Application.Abstractions.Users;
using MineHub.Application.Abstractions.Persistence;

namespace MineHub.Application.Carts.Queries.GetCart;

public class GetCartQueryHandler
{
    private readonly IDomainUserResolver _currentDomainUserService;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public GetCartQueryHandler(
        IDomainUserResolver currentDomainUserService, 
        IProductRepository productRepository,
        ICartRepository cartRepository)
    {
        _currentDomainUserService = currentDomainUserService;
        _productRepository = productRepository;
        _cartRepository = cartRepository;
    }

    public async Task<GetCartResponse> HandleAsync(CancellationToken token)
    {     
        var domainUser = await _currentDomainUserService.GetRequiredAsync(token);

        var cart = await _cartRepository.GetByUserIdAsync(domainUser.Id, token);

        if (cart is null)
            return new GetCartResponse(
                Guid.Empty, 
                null, 
                null, 
                0, 
                []);

        var productsIds = cart.CartItems.Select(c => c.ProductId).ToList();

        var products = await _productRepository.GetByIdsAsync(productsIds, token);

        var productsWithQuantity = products.Join(
            cart.CartItems,
            p => p.Id,
            c => c.ProductId,
            (p, c) => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                c.Quantity,
                TotalPrice = p.Price * c.Quantity
            }).ToList();

        return new GetCartResponse(
            cart.Id,
            cart.CreatedAtUtc,
            cart.UpdatedAtUtc,
            productsWithQuantity.Sum(p => p.TotalPrice),

            productsWithQuantity.Select(p =>
            new GetCartItemResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Quantity,
                p.TotalPrice)).ToList()
            );
    }
}
