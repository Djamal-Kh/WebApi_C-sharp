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
        Task<Result<Employee, Errors>> GetEmployeeByIdAsyncInAnimal(int employeeId, CancellationToken cancellationToken);

        Task<UnitResult<Error>> AssignAnimalToEmployeeAsync(int employeeId, int animalId, CancellationToken cancellationToken); 

        // Вынести в отдельный репозиторий - разделение Common и Queries (3 метода ниже)
        Task<List<Employee>> GetEmployeeWithoutAnimal(CancellationToken cancellationToken, Employee employee);
        Task GetEmployeeWithItsAnimals(int employeeId, CancellationToken cancellationToken); // подумать
        Task GetDatetimeSinceLastFeeding(int employeeId, CancellationToken cancellationToken); // подумать

        Task<UnitResult<Error>> PromotionEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> DemotionEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> RemoveAllBoundAnimalsAsync(int employeeId, CancellationToken cancellationToken); // дописать параметры и подумать

        // Переместить в AnimalRepository
        Task RemoveBoundAnimalAsync(int animalId, CancellationToken cancellationToken);

        Task<UnitResult<Error>> FireEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<Result<Employee, Error>> GetByIdAsync(int employeeId, CancellationToken cancellationToken);
    }
}
