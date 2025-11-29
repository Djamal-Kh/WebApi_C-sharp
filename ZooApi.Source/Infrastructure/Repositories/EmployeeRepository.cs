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
        public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
        }

        public async Task AssignAnimalToEmployee(int employeeId, int animalId, CancellationToken cancellation)
        {
            await context.Animals
                .Where(a => a.Id == animalId)
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(e => e.EmployeeId, employeeId));
        }

        public async Task FireEmployee(int id, CancellationToken cancellationToken)
        {
            await context.Animals
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<Result<string, Errors>> DemotionEmployee(Employee employee, CancellationToken cancellationToken)
        {
            string result = employee.Demotion();

            await context.SaveChangesAsync();
            return result;
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

        public async Task<Result<string, Errors>> PromotionEmployee(Employee employee, CancellationToken cancellationToken)
        {
            string result = employee.Promotion();

            await context.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task RemoveAllBoundAnimals(int employeeId, CancellationToken cancellationToken)
        {
            await context.Animals
                .Where(ei => ei.EmployeeId == employeeId)
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(e => e.EmployeeId, e => null));
        }

        public async Task RemoveBoundAnimal(int animalId, CancellationToken cancellationToken)
        {
            await context.Animals
                .Where(a => a.Id == animalId)
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(e => e.EmployeeId, e => null));
        }
    }
}
