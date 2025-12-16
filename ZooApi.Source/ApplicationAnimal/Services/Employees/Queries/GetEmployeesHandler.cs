using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using Dapper;
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

        public GetEmployeesHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<GetEmployeesDto> Handle(CancellationToken cancellationToken)
        {
            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            // SQL запрос для последующего использования Dapper`ом
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

            return new GetEmployeesDto(employees.ToList());
        }
    }
}
