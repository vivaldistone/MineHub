using Microsoft.Extensions.Caching.Distributed;
using MineHub.Application.Abstractions.Cache;

namespace MineHub.Infrastructure.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<string?> GetStringAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key is required", "key_is_required");
        
        return await _cache.GetStringAsync(key);
    }

    public async Task RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("key_is_required", "key_is_required");
        
        await _cache.RemoveAsync(key);
    }

    public async Task SetStringAsync(string key, string value, TimeSpan ttl)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("key is required");
        if (value is null)
            throw new ArgumentNullException(nameof(value));
        if (ttl <= TimeSpan.Zero)
            throw new ArgumentException("ttl must be greater than zero");

        var options = new DistributedCacheEntryOptions();
        options.AbsoluteExpirationRelativeToNow = ttl;

        await _cache.SetStringAsync(key, value, options);
    }
}
