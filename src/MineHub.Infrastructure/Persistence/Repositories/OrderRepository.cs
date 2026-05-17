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

    public async Task AddAsync(Order order)
    {
        _appDbContext.Orders.Add(order);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Order>> GetAllAsync()
    {
        return await _appDbContext.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetByUserIdAndOrderIdAsync(Guid userId, Guid orderId)
    {
        return await _appDbContext.Orders.FirstOrDefaultAsync(o => o.UserId == userId && o.Id == orderId);
    }

    public async Task<IReadOnlyCollection<Order>> GetByUserIdAsync(Guid userId)
    {
        return await _appDbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
    }

    public async Task<Order?> GetCreatedByUserIdAsync(Guid userId)
    {
        return await _appDbContext.Orders.FirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatus.Created);
    }

    public async Task UpdateAsync(Order order)
    {
        _appDbContext.Orders.Update(order);
        await _appDbContext.SaveChangesAsync();
    }
}
