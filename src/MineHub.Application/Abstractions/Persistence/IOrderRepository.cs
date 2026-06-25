using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken token);
    Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken token);
    Task<IReadOnlyCollection<Order>> GetByUserIdAsync(Guid userId, CancellationToken token);
    Task<Order?> GetByUserIdAndOrderIdAsync(Guid userId, Guid orderId, CancellationToken token);
    Task<Order?> GetCreatedByUserIdAsync(Guid userId, CancellationToken token);
    Task AddAsync(Order order, CancellationToken token);
    Task UpdateAsync(Order order, CancellationToken token); 
}
