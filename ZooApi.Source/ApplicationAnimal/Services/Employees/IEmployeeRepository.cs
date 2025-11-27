using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<Result<Employee, Errors>> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken);
        Task AssignAnimalToEmployee(CancellationToken cancellation); // дописать параметры
        Task<List<Employee>> GetEmployeeWithoutAnimal(CancellationToken cancellationToken, Employee employee);
        Task GetEmployeeWithItsAnimals(int employeeId, CancellationToken cancellationToken); // подумать
        Task GetDatetimeSinceLastFeeding(int employeeId, CancellationToken cancellationToken); // подумать
        Task<Result<string, Errors>> PromotionEmployee(int employeeId, CancellationToken cancellationToken);
        Task<Result<string, Errors>> DemotionEmployee(int employeeId, CancellationToken cancellationToken);
        Task RemoveBoundAnimals(int employeeId, CancellationToken cancellationToken); // дописать параметры и подумать
        Task<Result<string, Errors>> DeleteEmployee(int employeeId, CancellationToken cancellationToken);
    }
}
