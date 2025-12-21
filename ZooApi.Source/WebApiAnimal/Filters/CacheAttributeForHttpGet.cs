using ApplicationAnimal.Services.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAnimal.Filters
{
    public class CacheAttributeForHttpGet : ActionFilterAttribute
    {
        private readonly ILogger<CacheAttributeForHttpGet> _logger;
        private readonly IRedisCacheService _cache;
        public CacheAttributeForHttpGet(ILogger<CacheAttributeForHttpGet> logger, IRedisCacheService cache)
        {
            _logger = logger;
            _cache = cache;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            if (request.Method != HttpMethods.Get)
            {
                _logger.LogWarning("CacheAttributeForHttpGet be used for methods other than Get. Was an attempt to {Method}", request.Method);
                await next();
                return;
            }

            string cacheKey = $"cache:{request.Path.ToString().ToLower()}:{request.QueryString}";

            var cancellationToken = context.HttpContext.RequestAborted;
            var cacheData = await _cache.GetAsync<object>(cacheKey, cancellationToken);

            if (cacheData is not null)
            {
                _logger.LogInformation("Data taken from cache. Key: {CacheKey}", cacheKey);
                context.Result = new OkObjectResult(cacheData);
                return;
            }
            var executedContext = await next();

            if (executedContext.Result is OkObjectResult result && result.Value is not null)
            {
                _logger.LogInformation("Adding data to cache");
                await _cache.SetAsync(cacheKey, result.Value, cancellationToken);
            }
        }
    }
}
