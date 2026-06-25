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

    public async Task AddAsync(Cart cart, CancellationToken token)
    {
        await _appDbContext.Carts.AddAsync(cart, token);
    }

    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _appDbContext.Carts.FirstOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _appDbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId, token);
    }
}
