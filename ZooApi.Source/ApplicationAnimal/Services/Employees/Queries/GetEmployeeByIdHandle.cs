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
    public sealed class GetEmployeeByIdHandle
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetEmployeeByIdHandle(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<EmployeeDto?> Handle(int id, CancellationToken cancellationToken)
        {
            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            const string sql =
                $"""
                SELECT Id,
                    Name,
                    Position,
                    Limit
                FROM Employees
                WHERE Id = @EmployeeId
                """;

            var param = new { EmployeeId = id };

            var employee = await connection.QuerySingleOrDefaultAsync<EmployeeDto>(sql, param);

            return employee;
        }
    }
}
