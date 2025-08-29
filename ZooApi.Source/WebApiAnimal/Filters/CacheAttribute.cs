using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;


namespace WebApiAnimal.Filters
{
    public class CacheAttribute : ActionFilterAttribute
    {
        private readonly ILogger<CacheAttribute> _logger;

        public CacheAttribute(ILogger<CacheAttribute> logger)
        {
            _logger = logger;
        }

        public int DurationSeconds = 30;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var cacheKey = context.HttpContext.Request.Path;

            if (cache.TryGetValue(cacheKey, out var cachResult))
            {
                _logger.LogInformation("Данные взяты из кэша");
                context.Result = (IActionResult)cachResult;
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult result)
            {
                _logger.LogInformation("Добавление данных в кэш");
                cache.Set(cacheKey, result, TimeSpan.FromSeconds(DurationSeconds));
            }
        }
    }
}
