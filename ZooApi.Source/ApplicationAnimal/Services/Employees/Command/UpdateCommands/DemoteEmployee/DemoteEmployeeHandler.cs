using Shared.Common.Abstractions.Employees;
using Shared.Common.ResultPattern;
using Shared.Common.Extensions;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainAnimal;
using Microsoft.Extensions.Caching.Hybrid;

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.DemoteEmployee
{
    public sealed class DemoteEmployeeHandler : ICommandHandler<EnumEmployeePosition, DemoteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DemoteEmployeeHandler> _logger;
        private readonly IValidator<DemoteEmployeeCommand> _validator;
        private readonly HybridCache _cache;

        public DemoteEmployeeHandler(IEmployeeRepository employeeRepository, 
            ILogger<DemoteEmployeeHandler> logger, 
            IValidator<DemoteEmployeeCommand> validator,
            HybridCache cache)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
            _cache = cache;
        }

        public async Task<Result<EnumEmployeePosition, Errors>> Handle(DemoteEmployeeCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DemoteEmployeeCommand for EmployeeId: {EmployeeId}",
                command.employeeId);

            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for DemoteEmployeeCommand: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return validationResult.ToList();
            }

            // поиск сотрудника в БД
            var employee = await _employeeRepository.GetByIdAsync(command.employeeId, cancellationToken);

            if (employee is null)
            {
                _logger.LogWarning("Employee with Id {EmployeeId} not found", command.employeeId);
                return GeneralErrors.NotFound().ToErrors();
            }

            var oldPosition = employee.Position;

            // понижение сотрудника
            var result = await _employeeRepository.DemotionEmployeeAsync(employee, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to demote Employee with Id {EmployeeId}: {Error}",
                    command.employeeId, result.Error);

                return GeneralErrors.Failure("Ошибка при понижении сотрудника").ToErrors();
            }

            // инвалидация кэша
            var newPosition = employee.Position;

            var tags = new List<string> 
            { 
                EmployeeConstants.EMPLOYEE_CACHE_TAG,
                EmployeeConstants.EMPLOYEE_BY_ID_CACHE_TAG + employee.Id,
                EmployeeConstants.EMPLOYEES_BY_POSITION_CACHE_TAG + oldPosition,
                EmployeeConstants.EMPLOYEES_BY_POSITION_CACHE_TAG + newPosition,
            };

            await _cache.RemoveByTagAsync(tags, cancellationToken);

            // успешное понижение
            _logger.LogInformation("Successfully demoted Employee with Id {EmployeeId}", employee.Id);

            return employee.Position;
        }
    }
}
