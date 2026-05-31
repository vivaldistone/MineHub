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

    public async Task AddAsync(User user)
    {
        await _appDbContext.DomainUsers.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _appDbContext.DomainUsers.Remove(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _appDbContext.DomainUsers.AnyAsync(u => u.Id == id);
    }

    public async Task<IReadOnlyCollection<User>> GetAllAsync()
    {
        return await _appDbContext.DomainUsers.ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _appDbContext.DomainUsers.FirstOrDefaultAsync(u => u.Email ==  email);
    }

    public async Task<User?> GetByIdAsync(Guid guid)
    {
        return await _appDbContext.DomainUsers.FirstOrDefaultAsync(u => u.Id == guid);
    }

    public async Task<User?> GetByIdentityUserEmailAsync(string email)
    {
        return await _appDbContext.DomainUsers.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateAsync(User user)
    {
        _appDbContext.DomainUsers.Update(user);
        await _appDbContext.SaveChangesAsync();
    }
}
