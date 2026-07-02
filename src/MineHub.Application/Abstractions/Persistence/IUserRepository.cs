using MineHub.Domain.Entities;
using MineHub.Domain.ValueObjects;

namespace MineHub.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid guid, CancellationToken token);
    Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken token);
    Task<User?> GetByEmailAsync(EmailAdress email, CancellationToken token);
    Task<User?> GetByIdentityUserEmailAsync(EmailAdress authUserEmail, CancellationToken token);
    Task<bool> ExistsAsync(Guid id, CancellationToken token);
    Task AddAsync(User user, CancellationToken token);
    Task DeleteAsync(User user, CancellationToken token);
}
