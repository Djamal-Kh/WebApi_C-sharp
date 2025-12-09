using ApplicationAnimal.Common.ResultPattern;
using ApplicationAnimal.Services.Employees;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Infrastructure.ContextsDb;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UnitResult<Error>> AssignAnimalToEmployeeAsync(int employeeId, int animalId, CancellationToken cancellation)
        {
            var exists = await context.Animals
                .Where(a => a.Id == animalId)
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(e => e.EmployeeId, employeeId));

            if (exists == 0)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> FireEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            var exists = await context.Animals
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();

            if (exists == 0)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> DemotionEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            bool result = employee.Demotion();

            await context.SaveChangesAsync();
            
            if (!result)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> PromotionEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            bool result = employee.Promotion();

            await context.SaveChangesAsync(cancellationToken);
            
            if (!result)
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

        public async Task<Result<Employee, Error>> GetByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var employee = await context.Employees.FindAsync(employeeId, cancellationToken);
            
            if (employee == null)
                return GeneralErrors.NotFound();

            return employee;
        }
    }
}
