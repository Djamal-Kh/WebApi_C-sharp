using ApplicationAnimal.Common.Abstractions.Employees;
using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateEmployee
{
    public sealed class CreateEmployeeHandler : ICommandHandler<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<CreateEmployeeHandler> _logger;
        private readonly IValidator<CreateEmployeeCommand> _validator;
        public CreateEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<CreateEmployeeHandler> logger, IValidator<CreateEmployeeCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int, Errors>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new NotImplementedException();
            }

            // создание
            string name = command.CreateEmployeeRequest.Name;
            EnumEmployeePosition position = command.CreateEmployeeRequest.Position;

            Employee employee = new(name, position);

            // сохранение в БД
            var saveEmployee = await _employeeRepository.AddEmployeeAsync(employee, cancellationToken);
            if (saveEmployee.IsFailure)
                return GeneralErrors.ValueIsInvalid().ToErrors();

            return saveEmployee.Value.Id;
        }
    }
}
