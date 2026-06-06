namespace MineHub.Application.Abstractions.Services;

public interface ICacheService
{
    Task<string?> GetStringAsync(string key);
    Task SetStringAsync(string key, string value, TimeSpan ttl);
    Task RemoveAsync(string key);
}
