using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment, CancellationToken token);
    Task<List<Payment>> GetAllAsync(CancellationToken token);
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken token);
    Task<Payment?> GetPendingByOrderIdAsync(Guid orderId, CancellationToken token);
    Task<Payment?> GetByProviderPaymentIdAsync(string providerPaymentId, CancellationToken token);
}
