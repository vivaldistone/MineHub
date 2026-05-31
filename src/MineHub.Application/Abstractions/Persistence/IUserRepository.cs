using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid guid);
    Task<IReadOnlyCollection<User>> GetAllAsync();
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdentityUserEmailAsync(string identityUserEmail);
    Task<bool> ExistsAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}
