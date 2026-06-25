using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(Guid id, CancellationToken token);
    Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken token);
    Task AddAsync(Cart cart, CancellationToken token);
    Task UpdateAsync(Cart cart, CancellationToken token);
}
