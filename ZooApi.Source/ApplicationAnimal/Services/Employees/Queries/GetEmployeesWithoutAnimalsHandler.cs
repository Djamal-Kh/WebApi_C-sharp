using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Caching;
using Dapper;
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
        private readonly IRedisCacheService _cache;
        private readonly ILogger<GetEmployeesWithoutAnimalsHandler> _logger;

        public GetEmployeesWithoutAnimalsHandler(IDbConnectionFactory connectionFactory,
            IRedisCacheService cache,
            ILogger<GetEmployeesWithoutAnimalsHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _cache = cache;
            _logger = logger;
        }

        public async Task<GetEmployeesDto> Handle(CancellationToken cancellationToken)
        {
            var employeesFromCache = await _cache.GetAsync<List<EmployeeDto>>("employees_without_animals", cancellationToken);

            if (employeesFromCache is not null)
            {
                _logger.LogInformation("Employees without animals retrieved from cache.");
                return new GetEmployeesDto(employeesFromCache);
            }

            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            const string sql =
                """
                SELECT e.id,
                    e.name,
                    e.position,
                    e.animal_limit
                FROM employees e
                LEFT JOIN animals a ON e.id = a.employee_id
                Where a.employee_id IS NULL
                ORDER BY e.name
                """;

            var employees = await connection.QueryAsync<EmployeeDto>(sql);

            if (employees is not null)
            {
                _logger.LogInformation("Employees without animals added in cache");
                await _cache.SetAsync("employees_without_animals", employees.ToList(), cancellationToken);
            }    

            return new GetEmployeesDto(employees.ToList());
        }
    }
}
