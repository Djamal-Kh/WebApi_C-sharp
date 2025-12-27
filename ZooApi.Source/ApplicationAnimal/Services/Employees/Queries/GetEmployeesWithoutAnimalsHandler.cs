using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Caching;
using Dapper;
using DomainAnimal;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Queries
{
    public sealed class GetEmployeesWithoutAnimalsHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly HybridCache _cache;
        private readonly ILogger<GetEmployeesWithoutAnimalsHandler> _logger;

        public GetEmployeesWithoutAnimalsHandler(IDbConnectionFactory connectionFactory,
            HybridCache cache,
            ILogger<GetEmployeesWithoutAnimalsHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _cache = cache;
            _logger = logger;
        }

        public async Task<GetEmployeesDto> Handle(CancellationToken cancellationToken)
        {
            string cacheKey = "employee:employees_without_animals";

            var tags = new List<string>
            {
                EmployeeConstants.EMPLOYEE_CACHE_TAG,
                EmployeeConstants.EMPLOYEES_WITHOUT_ANIMALS 
            };

            var options = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(3)
            };

            var employees = await _cache.GetOrCreateAsync(
                cacheKey,
                async cancel =>
                {
                    _logger.LogInformation("Cache miss for key {CacheKey}", cacheKey);

                    var connection = await _connectionFactory.CreateConnectionAsync(cancel);
                    const string sql =
                        """
                        SELECT e.id,
                            e.name,
                            e.position,
                            e.animal_limit,
                            e.balance
                        FROM employees e
                        LEFT JOIN animals a ON e.id = a.employee_id
                        WHERE a.employee_id IS NULL
                        ORDER BY e.name
                        """;

                    var employees = await connection.QueryAsync<EmployeeDto>(sql);

                    _logger.LogInformation("Employees without animals retrieved from database and added to cache.");

                    return employees.ToList();
                },
                options,
                tags,
                cancellationToken: cancellationToken
            );

            return new GetEmployeesDto(employees);
        }
    }
}
