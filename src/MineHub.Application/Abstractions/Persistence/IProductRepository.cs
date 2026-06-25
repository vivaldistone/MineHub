using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken token);
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken token);
    Task<Product?> GetByNameAsync(string name, CancellationToken token);
    Task<bool> ExistsAsync(Guid id, CancellationToken token);
    Task AddAsync(Product product, CancellationToken token);
    Task UpdateAsync(Product product, CancellationToken token);
    Task DeleteAsync(Product product, CancellationToken token);
    Task<List<Product>> GetByIdsAsync(List<Guid> productIds, CancellationToken token);
}
