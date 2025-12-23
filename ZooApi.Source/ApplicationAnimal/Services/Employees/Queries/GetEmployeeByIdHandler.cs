using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Caching;
using Dapper;
using DomainAnimal;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace ApplicationAnimal.Services.Employees.Queries
{
    public sealed class GetEmployeeByIdHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly HybridCache _cache;
        private readonly ILogger<GetEmployeeByIdHandler> _logger;

        public GetEmployeeByIdHandler(IDbConnectionFactory connectionFactory, 
            HybridCache cache, 
            ILogger<GetEmployeeByIdHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _cache = cache;
            _logger = logger;
        }

        public async Task<EmployeeDto?> Handle(int id, CancellationToken cancellationToken)
        {
            string cacheKey = $"employee:id:{id}";

            var options = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(3)
            };

            var tags = new List<string> { EmployeeConstants.EMPLOYEE_BY_ID_CACHE_TAG + id };

            var employee = await _cache.GetOrCreateAsync<EmployeeDto?>(
                cacheKey,
                async cancel =>
                {
                    _logger.LogInformation("Cache MISS for key {CacheKey}", cacheKey);

                    using var connection = await _connectionFactory.CreateConnectionAsync(cancel);
                    const string sql =
                       """
                        SELECT id,
                            name,
                            position,
                            animal_limit
                        FROM employees
                        WHERE Id = @EmployeeId
                        """;

                    var param = new { EmployeeId = id };

                    var result = await connection.QuerySingleOrDefaultAsync<EmployeeDto>(sql, param);

                    return result;
                },
                options,
                tags,
                cancellationToken: cancellationToken
            );

            return employee;
        }
    }
}
