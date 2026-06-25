using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid guid, CancellationToken token);
    Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken token);
    Task<User?> GetByEmailAsync(string email, CancellationToken token);
    Task<User?> GetByIdentityUserEmailAsync(string identityUserEmail, CancellationToken token);
    Task<bool> ExistsAsync(Guid id, CancellationToken token);
    Task AddAsync(User user, CancellationToken token);
    Task DeleteAsync(User user, CancellationToken token);
}
