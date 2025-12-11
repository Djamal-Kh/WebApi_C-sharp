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

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.DemoteEmployee
{
    public sealed class DemoteEmployeeHandler : ICommandHandler<int, DemoteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DemoteEmployeeHandler> _logger;
        private readonly IValidator<DemoteEmployeeCommand> _validator;

        public DemoteEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<DemoteEmployeeHandler> logger, IValidator<DemoteEmployeeCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int, Errors>> Handle(DemoteEmployeeCommand command, CancellationToken cancellationToken)
        {
            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return validationResult.ToList();
            }

            var employee = await _employeeRepository.GetByIdAsync(command.employeeId, cancellationToken);

            if (employee is null)
            {
                return GeneralErrors.NotFound().ToErrors();
            }

            var result = await _employeeRepository.DemotionEmployeeAsync(employee, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError("Ошибка при понижении сотрудника с Id {EmployeeId}: {Error}", command.employeeId, result.Error);
                return GeneralErrors.Failure("Ошибка при понижении сотрудника").ToErrors();
            }

            return employee.Id;
        }
    }
}
