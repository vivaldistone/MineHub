using Microsoft.EntityFrameworkCore;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;
using MineHub.Domain.Enums;

namespace MineHub.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _appDbContext;

    public OrderRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(Order order, CancellationToken token)
    {
        _appDbContext.Orders.Add(order);
        await _appDbContext.SaveChangesAsync(token);
    }

    public async Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken token)
    {
        return await _appDbContext.Orders.ToListAsync(token);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _appDbContext.Orders.FirstOrDefaultAsync(o => o.Id == id, token);
    }

    public async Task<Order?> GetByUserIdAndOrderIdAsync(Guid userId, Guid orderId, CancellationToken token)
    {
        return await _appDbContext.Orders.FirstOrDefaultAsync(o => o.UserId == userId && o.Id == orderId, token);
    }

    public async Task<IReadOnlyCollection<Order>> GetByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _appDbContext.Orders.Where(o => o.UserId == userId).ToListAsync(token);
    }

    public async Task<Order?> GetCreatedByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _appDbContext.Orders.FirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatus.Created, token);
    }

    public async Task UpdateAsync(Order order, CancellationToken token)
    {
        _appDbContext.Orders.Update(order);
        await _appDbContext.SaveChangesAsync(token);
    }
}
