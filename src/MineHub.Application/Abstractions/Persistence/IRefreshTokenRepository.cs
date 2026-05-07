using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Persistence;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken> GetRefreshTokenAsync(string hash);
    Task UpdateAsync(RefreshToken token);
}
