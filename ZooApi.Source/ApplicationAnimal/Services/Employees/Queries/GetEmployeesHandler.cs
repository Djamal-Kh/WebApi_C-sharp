using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Caching;
using Dapper;
using DomainAnimal.Entities;
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
        private readonly IRedisCacheService _cache;
        private readonly ILogger<GetEmployeesHandler> _logger;

        public GetEmployeesHandler(IDbConnectionFactory connectionFactory,
            IRedisCacheService cache,
            ILogger<GetEmployeesHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _cache = cache;
            _logger = logger;
        }

        public async Task<GetEmployeesDto> Handle(CancellationToken cancellationToken)
        {
            // Проверка наличия данных в кэше
            var employeesFromCache = await _cache.GetAsync<List<EmployeeDto>>("employees_all", cancellationToken);

            if (employeesFromCache is not null)
            {
                _logger.LogInformation("All employees retrieved from cache");
                return new GetEmployeesDto(employeesFromCache);
            }
            // Подкючение к БД и SQL запрос с помощью Dapper`а
            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            const string sql = 
                """
                SELECT id, 
                    name, 
                    position, 
                    animal_limit 
                FROM employees
                ORDER BY Name;
                """;

            // Реализация запроса на получение сотрудников с использованием Dapper
            var employees = await connection.QueryAsync<EmployeeDto>(sql);

            if (employees is not null)
            {
                _logger.LogInformation("All employees added in cache");
                await _cache.SetAsync("employees_all", employees.ToList(), cancellationToken, 5, 3);
            }

            return new GetEmployeesDto(employees.ToList());
        }
    }
}
