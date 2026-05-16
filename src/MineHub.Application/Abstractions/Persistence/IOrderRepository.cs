using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Order>> GetAllAsync();
    Task<IReadOnlyCollection<Order>> GetByUserIdAsync(Guid userId);
    Task<Order?> GetByUserIdAndOrderIdAsync(Guid userId, Guid orderId);
    Task<Order?> GetCreatedByUserIdAsync(Guid userId);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}
