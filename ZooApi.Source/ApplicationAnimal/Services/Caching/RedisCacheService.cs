using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ApplicationAnimal.Services.Caching
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache? _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T? GetData<T>(string key)
        {
            var data = _cache?.GetString(key);

            if (data is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T data)
        {
            var optins = new DistributedCacheEntryOptions()
                // запись удалится через 10 минут независимо от активности
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                // запись удалится, если в течение 2 минут к ней не будет обращений
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache?.SetString(key, JsonSerializer.Serialize(data), optins);
        }
    }
}
