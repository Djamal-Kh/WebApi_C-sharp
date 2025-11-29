using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiAnimal.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class CacheAttribute : ActionFilterAttribute
    {
        private readonly ILogger<CacheAttribute> _logger;
        private readonly int DurationSeconds;
        public CacheAttribute(ILogger<CacheAttribute> logger, int durationSeconds = 30)
        {
            _logger = logger;
            DurationSeconds = durationSeconds;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var cacheKey = context.HttpContext.Request.Path;

            if (cache.TryGetValue(cacheKey, out var cachResult))
            {
                _logger.LogInformation("Data taken from cache");
                context.Result = (IActionResult)cachResult;
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult result)
            {
                _logger.LogInformation("Adding data to cache");
                cache.Set(cacheKey, result, TimeSpan.FromSeconds(DurationSeconds));
            }
        }
    }
}
