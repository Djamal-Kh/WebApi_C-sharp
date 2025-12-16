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
            _logger.LogInformation("Handling CreateEmployeeCommand for Name: {Name}",
                command.CreateEmployeeRequest.Name);

            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateEmployeeCommand: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return validationResult.ToList();
            }

            // бизнес-логика и создание
            string name = command.CreateEmployeeRequest.Name;

            bool isDuplicateName = await _employeeRepository.isDuplicateNameAsync(name, cancellationToken);

            if (isDuplicateName)
            {
                _logger.LogWarning("Duplicate employee name detected: {Name}", name);
                return GeneralErrors.ValueAlreadyExists(nameof(name)).ToErrors();
            }

            string toEnumPosition = command.CreateEmployeeRequest.Position;

            EnumEmployeePosition position = (EnumEmployeePosition)Enum.Parse(
                typeof(EnumEmployeePosition), toEnumPosition, ignoreCase: true);

            Employee employee = new(name, position);

            // сохранение новой сущности в БД
            var saveEmployee = await _employeeRepository.AddEmployeeAsync(employee, cancellationToken);

            if (saveEmployee.IsFailure)
            {
                _logger.LogError("Failed to save new employee: {Errors}",
                    string.Join(", ", saveEmployee.Error));

                return GeneralErrors.ValueIsInvalid().ToErrors();
            }
                
            _logger.LogInformation("Successfully created employee with ID: {Id}", saveEmployee.Value);
            return saveEmployee.Value;
        }
    }
}
