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

        public async Task<UnitResult<Error>> AssignAnimalToEmployee(int employeeId, int animalId, CancellationToken cancellation)
        {
            var exists = await context.Animals
                .Where(a => a.Id == animalId)
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(e => e.EmployeeId, employeeId));

            if (exists == 0)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> FireEmployee(int id, CancellationToken cancellationToken)
        {
            var exists = await context.Animals
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();

            if (exists == 0)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> DemotionEmployee(Employee employee, CancellationToken cancellationToken)
        {
            bool result = employee.Demotion();

            await context.SaveChangesAsync();
            
            if (!result)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task GetDatetimeSinceLastFeeding(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // Оставить без реализации - реализция отдельно через Dapper
        }

        public async Task<Result<Employee, Errors>> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // Оставить без реализации - реализция отдельно через Dapper
        }

        public async Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // Оставить без реализации - реализция отдельно через Dapper
        } 

        public async Task GetEmployeeWithItsAnimals(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // Оставить без реализации
        }

        public async Task<List<Employee>> GetEmployeeWithoutAnimal(CancellationToken cancellationToken, Employee employee)
        {
            throw new NotImplementedException(); // Оставить без реализации - реализция отдельно через Dapper
        }

        public async Task<UnitResult<Error>> PromotionEmployee(Employee employee, CancellationToken cancellationToken)
        {
            bool result = employee.Promotion();

            await context.SaveChangesAsync(cancellationToken);
            
            if (!result)
                return GeneralErrors.ValueIsInvalid();

            return UnitResult.Success<Error>();
        }

        public async Task<UnitResult<Error>> RemoveAllBoundAnimals(int employeeId, CancellationToken cancellationToken)
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
        public async Task RemoveBoundAnimal(int animalId, CancellationToken cancellationToken)
        {
            await context.Animals
                .Where(a => a.Id == animalId)
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(e => e.EmployeeId, e => null));
        }
    }
}
