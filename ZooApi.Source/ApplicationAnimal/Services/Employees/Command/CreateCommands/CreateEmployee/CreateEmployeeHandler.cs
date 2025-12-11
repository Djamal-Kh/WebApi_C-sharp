using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Abstractions.Employees;
using Shared.Common.ResultPattern;
using Shared.Common.Extensions;

namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateEmployee
{
    public sealed class CreateEmployeeHandler : ICommandHandler<int, CreateEmployeeCommand>
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
                return validationResult.ToList();
            }


            // создание
            string name = command.CreateEmployeeRequest.Name;
            EnumEmployeePosition position = command.CreateEmployeeRequest.Position;

            Employee employee = new(name, position);

            // сохранение в БД
            var saveEmployee = await _employeeRepository.AddEmployeeAsync(employee, cancellationToken);

            if (saveEmployee.IsFailure)
                return GeneralErrors.ValueIsInvalid().ToErrors();

            return saveEmployee.Value;
        }
    }
}
