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

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken token)
    {
        await _appDbContext.RefreshTokens.AddAsync(refreshToken, token);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string hash, CancellationToken token)
    {
        return await _appDbContext.RefreshTokens
            .FirstAsync(token => token.HashToken == hash, token);
    }

    public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken token)
    {
        _appDbContext.Update(refreshToken);
        await _appDbContext.SaveChangesAsync(token);
    }

    public async Task<RefreshToken?> GetTokenByUserAsync(string userId, CancellationToken token)
    {
        return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(token => token.UserId == userId, token);
    }
    
    public async Task DeleteAsync(RefreshToken refreshToken, CancellationToken token)
    {
        _appDbContext.Remove(refreshToken);
        await _appDbContext.SaveChangesAsync(token);
    }
}
