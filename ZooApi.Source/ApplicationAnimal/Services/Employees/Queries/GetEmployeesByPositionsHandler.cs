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

namespace ApplicationAnimal.Services.Employees.Queries
{
    // мб сделать валидацию входных параметров
    public sealed class GetEmployeesByPositionsHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IRedisCacheService _cache;
        private readonly ILogger<GetEmployeesByPositionsHandler> _logger;

        public GetEmployeesByPositionsHandler(IDbConnectionFactory connectionFactory, 
            IRedisCacheService cache,
            ILogger<GetEmployeesByPositionsHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _cache = cache;
            _logger = logger;
        }
        
        public async Task<GetEmployeesDto> Handle(EnumEmployeePosition position, CancellationToken cancellationToken)
        {
            var employeesFromCache = await _cache.GetAsync<List<EmployeeDto>>($"employees_position:{position}", cancellationToken);

            if (employeesFromCache is not null)
            {
                _logger.LogInformation("Employees with position {position} retrieved from cache", position);
                return new GetEmployeesDto(employeesFromCache);
            }
                

            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            const string sql =
                """
                SELECT id,
                    name,
                    position,
                    animal_limit
                FROM employees
                Where position = @EmployeePosition
                """;
            
            var param = new {EmployeePosition = position.ToString()};

            var employees = await connection.QueryAsync<EmployeeDto>(sql, param);

            if (employees.Any())
            {
                _logger.LogInformation("Employees with position {position} added in cache", position);
                await _cache.SetAsync($"employees_position:{position}", employees.ToList(), cancellationToken, 5, 3);
            }

            return new GetEmployeesDto(employees.ToList());
        }
    }
}
