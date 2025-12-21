using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationAnimal.Services.Caching
{
    public interface IRedisCacheService
    {
        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
        public Task SetAsync<T>(string key, T data, CancellationToken cancellationToken, int absoluteTTL = 3, int slidingTTL = 1);
        public Task RemoveDataAsync(string key);
    }
}
