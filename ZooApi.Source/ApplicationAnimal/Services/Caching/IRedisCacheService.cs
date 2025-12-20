using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationAnimal.Services.Caching
{
    public interface IRedisCacheService
    {
        T? GetData<T>(string key);
        void SetData<T>(string key, T data);
    }
}
