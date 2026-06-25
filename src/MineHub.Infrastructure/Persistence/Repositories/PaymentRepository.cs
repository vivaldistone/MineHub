using Microsoft.EntityFrameworkCore;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;
using MineHub.Domain.Enums;

namespace MineHub.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _appDbContext;

    public PaymentRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(Payment payment, CancellationToken token)
    {
        await _appDbContext.Payments.AddAsync(payment, token);
    }

    public async Task<List<Payment>> GetAllAsync(CancellationToken token)
    {
        return await _appDbContext.Payments
            .ToListAsync(token);
    }

    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _appDbContext.Payments
            .FirstOrDefaultAsync(p => p.Id == id, token);
    }

    public async Task<Payment?> GetPendingByOrderIdAsync(Guid orderId, CancellationToken token)
    {
        return await _appDbContext.Payments
            .FirstOrDefaultAsync(p =>
                p.OrderId == orderId &&
                p.Status == PaymentStatus.Pending, token);
    }

    public async Task<Payment?> GetByProviderPaymentIdAsync(string providerPaymentId, CancellationToken token)
    {
        return await _appDbContext.Payments
            .FirstOrDefaultAsync(p =>
                p.ProviderPaymentId == providerPaymentId, token);
    }
}
