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
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
               return validationResult.ToList();
            }

            Animal animal = await _animalRepository.GetAnimalByIdAsync(command.animalId, cancellationToken);

            if (animal == null)
                return GeneralErrors.NotFound().ToErrors();

            Employee employee = await _employeeRepository.GetByIdWithAnimalsAsync(command.employeeId, cancellationToken);

            if (employee == null)
                return GeneralErrors.NotFound().ToErrors();

            var result = await _employeeRepository.AssignAnimalToEmployeeAsync(employee, animal, cancellationToken);

            if (result.IsFailure)
                return GeneralErrors.ValueIsInvalid().ToErrors();

            return $"Employee с id {command.employeeId} успешно привязан к Animal с id {command.animalId}";
        }
    }
}
