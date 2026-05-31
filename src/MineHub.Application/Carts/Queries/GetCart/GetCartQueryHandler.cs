using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;

namespace MineHub.Application.Carts.Queries.GetCart;

public class GetCartQueryHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly ICurrentDomainUserService _currentDomainUserService;

    public GetCartQueryHandler(ICartRepository cartRepository, ICurrentDomainUserService currentDomainUserService)
    {
        _cartRepository = cartRepository;
        _currentDomainUserService = currentDomainUserService;
    }

    public async Task<GetCartResponse> HandleAsync()
    {
        var domainUser = await _currentDomainUserService.GetRequiredAsync();

        var cart = await _cartRepository.GetByUserIdAsync(domainUser.Id);

        if (cart is null)
        {
            return new GetCartResponse(
                Guid.Empty,
                DateTime.UtcNow,
                DateTime.UtcNow,
                0,
                []);
        }

        return new GetCartResponse(
            cart.Id, 
            cart.CreatedAtUtc, 
            cart.UpdatedAtUtc,
            cart.TotalPrice,

            cart.CartItems.Select(c => new GetCartItemResponse(
            c.ProductId,
            c.ProductName,
            c.Description,
            c.UnitPrice,
            c.Quantity,
            c.TotalPrice)).ToList()
            );
    }
}
