using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Common.Abstractions.Employees
{
    public interface IEmployeeRepository
    {
        Task<Result<int, Error>> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> AssignAnimalToEmployeeAsync(int employeeId, int animalId, CancellationToken cancellationToken); 
        Task<UnitResult<Error>> PromotionEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> DemotionEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> RemoveAllBoundAnimalsAsync(int employeeId, CancellationToken cancellationToken); 
        Task<UnitResult<Error>> FireEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<Result<Employee, Error>> GetByIdAsync(int employeeId, CancellationToken cancellationToken);

        // Все методы ниже вынести отсюда !!!! 

        // Вынести в отдельный репозиторий - разделение Common и Queries
        Task<List<Employee>> GetEmployeeWithoutAnimal(CancellationToken cancellationToken, Employee employee);
        Task GetEmployeeWithItsAnimals(int employeeId, CancellationToken cancellationToken); // подумать над параметрами
        Task GetDatetimeSinceLastFeeding(int employeeId, CancellationToken cancellationToken); // подумать над параметрами
        Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<Result<Employee, Errors>> GetEmployeeByIdAsyncInAnimal(int employeeId, CancellationToken cancellationToken);        
    }
}
