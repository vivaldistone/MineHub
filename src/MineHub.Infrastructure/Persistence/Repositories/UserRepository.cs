using Microsoft.EntityFrameworkCore;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(User user, CancellationToken token)
    {
        await _appDbContext.DomainUsers.AddAsync(user, token);
        await _appDbContext.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(User user, CancellationToken token)
    {
        _appDbContext.DomainUsers.Remove(user);
        await _appDbContext.SaveChangesAsync(token);
    }

    public async Task<bool> ExistsAsync(Guid id , CancellationToken token)
    {
        return await _appDbContext.DomainUsers.AnyAsync(u => u.Id == id, token);
    }

    public async Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken token)
    {
        return await _appDbContext.DomainUsers.ToListAsync(token);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token)
    {
        return await _appDbContext.DomainUsers.FirstOrDefaultAsync(u => u.Email ==  email, token);
    }

    public async Task<User?> GetByIdAsync(Guid guid, CancellationToken token)
    {
        return await _appDbContext.DomainUsers.FirstOrDefaultAsync(u => u.Id == guid, token);
    }

    public async Task<User?> GetByIdentityUserEmailAsync(string email, CancellationToken token)
    {
        return await _appDbContext.DomainUsers.FirstOrDefaultAsync(u => u.Email == email, token);
    }

    public async Task UpdateAsync(User user, CancellationToken token)
    {
        _appDbContext.DomainUsers.Update(user);
        await _appDbContext.SaveChangesAsync(token);
    }
}
