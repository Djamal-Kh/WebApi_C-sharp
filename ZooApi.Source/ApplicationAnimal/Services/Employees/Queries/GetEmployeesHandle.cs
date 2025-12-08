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
    public sealed class GetEmployeesHandlerDapper
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetEmployeesHandlerDapper(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<GetEmployeesDto> Handle(CancellationToken cancellationToken)
        {
            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            // SQL запрос для последующего использования Dapper`ом
            const string sql = 
                """
                SELECT e.Id, 
                    Name, 
                    Position, 
                    Limit 
                FROM Employees
                ORDER BY Name;
                """;

            // Реализация запроса на получение сотрудников с использованием Dapper
            var employees = await connection.QueryAsync<EmployeeDto>(sql);

            return new GetEmployeesDto(employees.ToList());
        }
    }
}
