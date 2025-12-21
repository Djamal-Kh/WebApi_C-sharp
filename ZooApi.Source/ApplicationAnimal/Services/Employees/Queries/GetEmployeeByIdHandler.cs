using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Caching;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ApplicationAnimal.Services.Employees.Queries
{
    public sealed class GetEmployeeByIdHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IRedisCacheService _cache;
        private readonly ILogger<GetEmployeeByIdHandler> _logger;

        public GetEmployeeByIdHandler(IDbConnectionFactory connectionFactory, IRedisCacheService cache, ILogger<GetEmployeeByIdHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _cache = cache;
            _logger = logger;
        }

        public async Task<EmployeeDto?> Handle(int id, CancellationToken cancellationToken)
        {
            // Кэширование сотрудника по id
            var employeeFromCache = await _cache.GetAsync<EmployeeDto>($"employee:id:{id}", cancellationToken);

            if (employeeFromCache is not null)
            {
                _logger.LogInformation("Employee with id {EmployeeId} retrieved from cache", id);
                return employeeFromCache;
            }

            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            const string sql =
                $"""
                SELECT id,
                    name,
                    position,
                    animal_limit
                FROM employees
                WHERE Id = @EmployeeId
                """;

            var param = new { EmployeeId = id };

            var employee = await connection.QuerySingleOrDefaultAsync<EmployeeDto>(sql, param);

            if (employee is not null)
            {
                _logger.LogInformation("Employee with id {EmployeeId} added in cache", id);
                await _cache.SetAsync($"employee:id:{id}", employee, cancellationToken);
            }

            return employee;
        }
    }
}
