using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ApplicationAnimal.Services.Caching
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            var data = await _cache.GetStringAsync(key, cancellationToken);

            if (data is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task SetAsync<T>(string key, T data, CancellationToken cancellationToken, int absoluteTTL = 3, int slidingTTL = 1)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteTTL))
                .SetSlidingExpiration(TimeSpan.FromMinutes(slidingTTL));

            await _cache.SetStringAsync(key, JsonSerializer.Serialize(data), options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }
    }
}
