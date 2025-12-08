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
    public sealed class GetEmployeesWithoutAnimalsHandle
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetEmployeesWithoutAnimalsHandle(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<GetEmployeesDto> Handle(CancellationToken cancellationToken)
        {
            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            const string sql =
                """
                SELECT Id,
                    e.Name,
                    e.Position,
                    e.Limit,
                From Employees e
                LEFT JOIN Animals a ON e.Id = a.EmployeeId
                Where a.EmployeeId IS NULL
                ORDER BY e.Name
                """;

            var employees = await connection.QueryAsync<EmployeeDto>(sql);

            return new GetEmployeesDto(employees.ToList());
        }
    }
}
