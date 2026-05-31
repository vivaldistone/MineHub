namespace MineHub.Application.Abstractions.Services;

public interface IIdentityService
{
    Task<(bool Success, IEnumerable<string> Errors, string UserId)> CreateUserAsync(string email, string password);
    Task<LoginResult> LoginAsync(string email, string password);
    Task<TokenUserInfo> GetTokenUserInfoAsync(string authUserId);
}
