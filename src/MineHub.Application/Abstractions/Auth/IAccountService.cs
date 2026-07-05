using MineHub.Application.Auth.DTOs;

namespace MineHub.Application.Abstractions.Auth;

public interface IAccountService
{
    Task<(bool Success, IEnumerable<string> Errors, string UserId)> CreateUserAsync(string email, string password, CancellationToken token);
    Task<LoginResult> LoginAsync(string email, string password, CancellationToken token);
    Task<TokenUserInfo> GetTokenUserInfoAsync(string authUserId, CancellationToken token);
    Task RequestPasswordResetAsync(string email, CancellationToken token);
    Task ResetPasswordAsync(string email, string tokenReset, string newPassword, CancellationToken token);
}
