using ApplicationAnimal.Common.ResultPattern;
using ApplicationAnimal.Services.Employees;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public sealed class EmployeeRepository : IEmployeeRepository
    {
        public Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AssignAnimalToEmployee(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string, Errors>> DeleteEmployee(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string, Errors>> DemotionEmployee(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task GetDatetimeSinceLastFeeding(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Employee, Errors>> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task GetEmployeeWithItsAnimals(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Employee>> GetEmployeeWithoutAnimal(CancellationToken cancellationToken, Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string, Errors>> PromotionEmployee(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBoundAnimals(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
