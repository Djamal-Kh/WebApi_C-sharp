using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Abstractions.Employees;
using Shared.Common.ResultPattern;
using Shared.Common.Extensions;
using ApplicationAnimal.Services.Animals;
using DomainAnimal.Entities;

namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal
{
    public sealed class CreateBoundWithAnimalHandler : ICommandHandler<string, CreateBoundWithAnimalCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly ILogger<CreateBoundWithAnimalHandler> _logger;
        private readonly IValidator<CreateBoundWithAnimalCommand> _validator;

        public CreateBoundWithAnimalHandler(IEmployeeRepository employeeRepository, IAnimalRepository animalRepository, ILogger<CreateBoundWithAnimalHandler> logger, IValidator<CreateBoundWithAnimalCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _animalRepository = animalRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<string, Errors>> Handle(CreateBoundWithAnimalCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateBoundWithAnimalCommand for EmployeeId: {EmployeeId} and AnimalId: {AnimalId}",
                command.employeeId, command.animalId);

            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateBoundWithAnimalCommand: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return validationResult.ToList();
            }

            // поиск сущностей в БД
            Animal? animal = await _animalRepository.GetAnimalByIdAsync(command.animalId, cancellationToken);

            if (animal == null)
            {
                _logger.LogWarning("Animal with Id {AnimalId} not found.", 
                    command.animalId);

                return GeneralErrors.NotFound().ToErrors();
            }
                
            Employee? employee = await _employeeRepository.GetByIdWithAnimalsAsync(command.employeeId, cancellationToken);

            if (employee == null)
            {
                _logger.LogWarning("Employee with Id {EmployeeId} not found.", 
                    command.employeeId);

                return GeneralErrors.NotFound().ToErrors(); 
            }

            // привязка животного к сотруднику
            var result = await _employeeRepository.AssignAnimalToEmployeeAsync(employee, animal, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Failed to assign Animal with Id {AnimalId} to Employee with Id {EmployeeId}",
                    command.animalId, command.employeeId);

                return GeneralErrors.ValueIsInvalid().ToErrors();
            }

            _logger.LogInformation("Successfully assigned Animal with Id {AnimalId} to Employee with Id {EmployeeId}",
                        command.animalId, command.employeeId);

            return $"Employee с id {command.employeeId} успешно привязан к Animal с id {command.animalId}";
        }
    }
}
