using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface ICartRepository
{
    Task SaveAsync(Cart cart, CancellationToken token);
    Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken token);
    Task RemoveAsync(Guid userId, CancellationToken token);
}
