using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using DomainAnimal.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationAnimal.Services.Caching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Hybrid;
using DomainAnimal;

namespace ApplicationAnimal.Services.Employees.Queries
{
    // мб сделать валидацию входных параметров
    public sealed class GetEmployeesByPositionsHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly HybridCache _cache;
        private readonly ILogger<GetEmployeesByPositionsHandler> _logger;

        public GetEmployeesByPositionsHandler(IDbConnectionFactory connectionFactory, 
            HybridCache cache,
            ILogger<GetEmployeesByPositionsHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _cache = cache;
            _logger = logger;
        }
        
        public async Task<GetEmployeesDto> Handle(EnumEmployeePosition position, CancellationToken cancellationToken)
        {
            string cacheKey = $"employee:employees_position:{position}";

            var options = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(3), 

            };

            var tags = new List<string> { EmployeeConstants.EMPLOYEE_CACHE_TAG };

            var employees = await _cache.GetOrCreateAsync<List<EmployeeDto>>(
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
                        Where position = @EmployeePosition
                        """;

                    var param = new { EmployeePosition = position.ToString() };

                    var result = await connection.QueryAsync<EmployeeDto>(sql, param);
                    
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
