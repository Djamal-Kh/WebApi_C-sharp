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
    public sealed class GetEmployeeByIdHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetEmployeeByIdHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<EmployeeDto?> Handle(int id, CancellationToken cancellationToken)
        {
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

            return employee;
        }
    }
}
