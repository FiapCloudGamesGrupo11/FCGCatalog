using System.Text.Json;
using FiapCloudGames.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace FiapCloudGames.Infrastructure.Persistence.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cached = await _cache.GetStringAsync(key);
            return cached is null ? default : JsonSerializer.Deserialize<T>(cached);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serialized = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }

        public Task RemoveAsync(string key)
        {
            return _cache.RemoveAsync(key);
        }
    }
}
