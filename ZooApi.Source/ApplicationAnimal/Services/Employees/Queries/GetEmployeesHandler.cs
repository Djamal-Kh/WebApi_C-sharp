using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Caching;
using Dapper;
using DomainAnimal;
using DomainAnimal.Entities;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Queries
{
    public sealed class GetEmployeesHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly HybridCache _cache;
        private readonly ILogger<GetEmployeesHandler> _logger;

        public GetEmployeesHandler(IDbConnectionFactory connectionFactory,
            HybridCache cache,
            ILogger<GetEmployeesHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _cache = cache;
            _logger = logger;
        }

        public async Task<GetEmployeesDto> Handle(CancellationToken cancellationToken)
        {
            string cacheKey = "employee:employees_all";

            var options = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(3)
            };

            var tags = new List<string> { EmployeeConstants.EMPLOYEE_CACHE_TAG };

            var employees = await _cache.GetOrCreateAsync(
                cacheKey,
                async cancel =>
                {
                    _logger.LogInformation("Cache miss for key {CacheKey}. Retrieving from database.", cacheKey);

                    var connection = await _connectionFactory.CreateConnectionAsync(cancel);
                    const string sql =
                        """
                        SELECT id, 
                            name, 
                            position, 
                            animal_limit 
                        FROM employees
                        ORDER BY Name;
                        """;

                    var result = await connection.QueryAsync<EmployeeDto>(sql);

                    return result.ToList();
                },
                options,
                tags,
                cancellationToken: cancellationToken
            );

            return new GetEmployeesDto(employees);
        }
    }
}
