using Microsoft.EntityFrameworkCore;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _appDbContext;

    public CartRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(Cart cart)
    {
        await _appDbContext.Carts.AddAsync(cart);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Cart?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Carts.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId)
    {
        return await _appDbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task UpdateAsync(Cart cart)
    {
        _appDbContext.Carts.Update(cart);
        await _appDbContext.SaveChangesAsync();
    }
}
