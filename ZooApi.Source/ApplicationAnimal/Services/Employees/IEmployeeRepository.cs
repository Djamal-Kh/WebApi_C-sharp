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
    // Добавь к названию всех методов ASYNC
    public interface IEmployeeRepository
    {
        Task<Result<int, Error>> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);

        // Вынести в отдельный репозиторий - разделение Common и Queries (2 метода ниже)
        Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<Result<Employee, Errors>> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken);

        Task<UnitResult<Error>> AssignAnimalToEmployee(int employeeId, int animalId, CancellationToken cancellationToken); 

        // Вынести в отдельный репозиторий - разделение Common и Queries (3 метода ниже)
        Task<List<Employee>> GetEmployeeWithoutAnimal(CancellationToken cancellationToken, Employee employee);
        Task GetEmployeeWithItsAnimals(int employeeId, CancellationToken cancellationToken); // подумать
        Task GetDatetimeSinceLastFeeding(int employeeId, CancellationToken cancellationToken); // подумать

        Task<UnitResult<Error>> PromotionEmployee(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> DemotionEmployee(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> RemoveAllBoundAnimals(int employeeId, CancellationToken cancellationToken); // дописать параметры и подумать

        // Переместить в AnimalRepository
        Task RemoveBoundAnimal(int animalId, CancellationToken cancellationToken);

        Task<UnitResult<Error>> FireEmployee(int id, CancellationToken cancellationToken);
    }
}
