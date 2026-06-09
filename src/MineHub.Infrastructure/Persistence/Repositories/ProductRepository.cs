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


    public async Task AddAsync(Product product)
    {
        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _appDbContext.Products.Remove(product);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _appDbContext.Products.AnyAsync(p => p.ProductId == id);
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync()
    {
        return await _appDbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
        return await _appDbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task UpdateAsync(Product product)
    {
        _appDbContext.Products.Update(product);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<Product>> GetByIdsAsync(List<Guid> productIds)
    {
        return await _appDbContext
            .Products
            .Where(p => productIds.Contains(p.ProductId))
            .ToListAsync();
    }
}
