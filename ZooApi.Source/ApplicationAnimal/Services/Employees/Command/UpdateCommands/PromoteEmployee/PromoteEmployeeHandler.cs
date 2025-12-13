using Shared.Common.Abstractions.Employees;
using Shared.Common.ResultPattern;
using Shared.Common.Extensions;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.PromoteEmployee
{
    public sealed class PromoteEmployeeHandler : ICommandHandler<int, PromoteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<PromoteEmployeeHandler> _logger;
        private readonly IValidator<PromoteEmployeeCommand> _validator;

        public PromoteEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<PromoteEmployeeHandler> logger, IValidator<PromoteEmployeeCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int, Errors>> Handle(PromoteEmployeeCommand command, CancellationToken cancellationToken)
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

            var employee = await _employeeRepository.GetByIdAsync(command.employeeId, cancellationToken);

            if (employee is null)
            {
                _logger.LogWarning("Employee with Id {EmployeeId} not found", command.employeeId);

                return GeneralErrors.NotFound().ToErrors();
            }

            var result = await _employeeRepository.PromotionEmployeeAsync(employee, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to promote Employee with Id {EmployeeId}: {Error}",
                    command.employeeId, result.Error);

                return GeneralErrors.Failure("Ошибка при повышении сотрудника").ToErrors();
            }

            _logger.LogInformation("Successfully promoted Employee with Id {EmployeeId}", employee.Id);
            return employee.Id;
        }
    }
}
