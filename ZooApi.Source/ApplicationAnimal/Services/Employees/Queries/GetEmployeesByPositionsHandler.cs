using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using DomainAnimal.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Queries
{
    // мб сделать валидацию входных параметров
    public sealed class GetEmployeesByPositionsHandler
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetEmployeesByPositionsHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        
        public async Task<GetEmployeesDto> Handle(EnumEmployeePosition position, CancellationToken cancellationToken)
        {
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

            return new GetEmployeesDto(employees.ToList());
        }
    }
}
