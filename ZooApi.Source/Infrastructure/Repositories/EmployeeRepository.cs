using ApplicationAnimal.Services.Employees;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Infrastructure.ContextsDb;
using Microsoft.EntityFrameworkCore;
using Shared.Common.ResultParttern;
using Shared.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public sealed class EmployeeRepository(AppContextDB context) : IEmployeeRepository
    {
        public async Task<Result<int, Error>> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            try
            {
                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                return employee.Id;
            }

            catch 
            {
                return GeneralErrors.ValueIsInvalid();
            }
        }

        /* Да, понимаю что это очень странный костыль
         * Но это заготовка под возможный переход на DDD. 
         * В случае перехода на него, будет довольно легко это сделать
         * А пока оставим как есть. По идее должно работать
        */
        public async Task<UnitResult<Errors>> AssignAnimalToEmployeeAsync(Employee employee, Animal animal, CancellationToken cancellationToken)
        {
            var result = employee.AssignAnimal(animal);

            if (result.IsFailure)
                return result.Error.ToErrors();

            await context.SaveChangesAsync();

            return UnitResult.Success<Errors>();
        }

        public async Task<UnitResult<Error>> FireEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            var exists = await context.Employees
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();

            if (exists == 0)
                return GeneralErrors.NotFound();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> DemotionEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            var result = employee.Demotion();

            await context.SaveChangesAsync();
            
            if (result.IsFailure)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> PromotionEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            var result = employee.Promotion();

            await context.SaveChangesAsync(cancellationToken);
            
            if (result.IsFailure)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> RemoveAllBoundAnimalsAsync(int employeeId, CancellationToken cancellationToken)
        {
            var exists = await context.Animals
                .Where(ei => ei.EmployeeId == employeeId)
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(e => e.EmployeeId, e => null));

            if(exists == 0)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        // Переместить в AnimalRepository
        public async Task RemoveBoundAnimalAsync(int animalId, CancellationToken cancellationToken)
        {
            await context.Animals
                .Where(a => a.Id == animalId)
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(e => e.EmployeeId, e => null));
        }

        public async Task<Employee?> GetByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var employee = await context.Employees.FindAsync(employeeId, cancellationToken);
            return employee;
        }

        public async Task<Employee?> GetByIdWithAnimalsAsync(int employeeId, CancellationToken cancellationToken)
        {
            var employee = await context.Employees
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            return employee;
        }

        public async Task<bool> isDuplicateNameAsync(string name, CancellationToken cancellationToken = default)
        {
            bool exist = await context.Employees.AnyAsync(n => n.Name == name);
            return exist;
        }
    }
}
