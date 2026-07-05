using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;

namespace MineHub.Application.Carts.Commands.ClearCart;

public class ClearCartCommandHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly IDomainUserResolver _domainUserResolver;
    public ClearCartCommandHandler(
        ICartRepository cartRepository, 
        IDomainUserResolver domainUserResolver)
    {
        _cartRepository = cartRepository;
        _domainUserResolver = domainUserResolver;
    }

    public async Task HandleAsync(CancellationToken token)
    {
        var domainUser = await _domainUserResolver.GetRequiredAsync(token);

        await _cartRepository.RemoveAsync(domainUser.Id, token);
    }
}
