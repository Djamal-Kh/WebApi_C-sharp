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
    public sealed class GetEmployeesWithoutAnimalsHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetEmployeesWithoutAnimalsHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<GetEmployeesDto> Handle(CancellationToken cancellationToken)
        {
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

            return new GetEmployeesDto(employees.ToList());
        }
    }
}
