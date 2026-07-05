using Microsoft.EntityFrameworkCore;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(Product product, CancellationToken token)
    {
        await _appDbContext.Products.AddAsync(product, token);
    }

    public async Task DeleteAsync(Product product, CancellationToken token)
    {
        _appDbContext.Products.Remove(product);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken token)
    {
        return await _appDbContext.Products.AnyAsync(p => p.Id == id, token);
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken token)
    {
        return await _appDbContext.Products.ToListAsync(token);
    }

    public async Task<Product?> GetByIdAsync(Guid id , CancellationToken token)
    {
        return await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == id, token);
    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken token)
    {
        return await _appDbContext.Products.FirstOrDefaultAsync(p => p.Name == name, token);
    }

    public async Task<List<Product>> GetByIdsAsync(List<Guid> productIds, CancellationToken token)
    {
        return await _appDbContext
            .Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(token);
    }
}
