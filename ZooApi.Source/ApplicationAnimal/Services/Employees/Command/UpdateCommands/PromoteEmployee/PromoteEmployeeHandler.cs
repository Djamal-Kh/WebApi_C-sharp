using CSharpFunctionalExtensions;
using DomainAnimal;
using DomainAnimal.Entities;
using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Common.Abstractions.Employees;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.PromoteEmployee
{
    public sealed class PromoteEmployeeHandler : ICommandHandler<EnumEmployeePosition, PromoteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<PromoteEmployeeHandler> _logger;
        private readonly IValidator<PromoteEmployeeCommand> _validator;
        private readonly HybridCache _cache;

        public PromoteEmployeeHandler(IEmployeeRepository employeeRepository,
            ILogger<PromoteEmployeeHandler> logger, 
            IValidator<PromoteEmployeeCommand> validator,
            HybridCache cache)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
            _cache = cache;
        }

        public async Task<Result<EnumEmployeePosition, Errors>> Handle(PromoteEmployeeCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling PromoteEmployeeCommand for EmployeeId: {EmployeeId}",
                command.employeeId);

            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for PromoteEmployeeCommand: {Errors}",
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

            // повышение сотрудника
            var result = await _employeeRepository.PromotionEmployeeAsync(employee, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to promote Employee with Id {EmployeeId}: {Error}",
                    command.employeeId, result.Error);

                return GeneralErrors.Failure("Ошибка при повышении сотрудника").ToErrors();
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

            // успешное повышение
            _logger.LogInformation("Successfully promoted Employee with Id {EmployeeId}", employee.Id);
            return employee.Position;
        }
    }
}
