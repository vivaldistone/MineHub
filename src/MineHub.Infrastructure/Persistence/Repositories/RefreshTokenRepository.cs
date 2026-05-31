using Microsoft.EntityFrameworkCore;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _appDbContext;

    public RefreshTokenRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _appDbContext.RefreshTokens.AddAsync(refreshToken);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string hash)
    {
        return await _appDbContext.RefreshTokens
            .FirstAsync(token => token.HashToken == hash);
    }

    public async Task UpdateAsync(RefreshToken token)
    {
        _appDbContext.Update(token);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetTokenByUserAsync(string userId)
    {
        return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(token => token.UserId == userId);
    }
    
    public async Task DeleteAsync(RefreshToken refreshToken)
    {
        _appDbContext.Remove(refreshToken);
        await _appDbContext.SaveChangesAsync();
    }
}
