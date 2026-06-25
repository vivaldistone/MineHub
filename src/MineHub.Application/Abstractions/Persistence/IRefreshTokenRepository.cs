using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken token);
    Task<RefreshToken> GetRefreshTokenAsync(string hash, CancellationToken token);
    Task<RefreshToken?> GetTokenByUserAsync(string userId, CancellationToken token);
    Task DeleteAsync(RefreshToken refreshToken, CancellationToken token);
}