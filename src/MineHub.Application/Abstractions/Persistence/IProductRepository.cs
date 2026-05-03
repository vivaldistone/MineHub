using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Product>> GetAllAsync();
    Task<Product?> GetByNameAsync(string name);
    Task<bool> ExistsAsync(Guid id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
