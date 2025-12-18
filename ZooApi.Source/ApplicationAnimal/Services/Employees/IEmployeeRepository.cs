using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Shared.Common.ResultParttern;
using Shared.Common.ResultPattern;

namespace ApplicationAnimal.Services.Employees
{
    public interface IEmployeeRepository
    {
        Task<UnitResult<Errors>> AssignAnimalToEmployeeAsync(Employee employee, Animal animal, CancellationToken cancellationToken);
        Task<Result<int, Error>> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> PromotionEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> DemotionEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<UnitResult<Error>> RemoveAllBoundAnimalsAsync(int employeeId, CancellationToken cancellationToken); 
        Task<UnitResult<Error>> FireEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<Employee?> GetByIdAsync(int employeeId, CancellationToken cancellationToken);  
        Task<Employee?> GetByIdWithAnimalsAsync(int empId, CancellationToken cancellationToken);
        Task<bool> isDuplicateNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
