using ApplicationAnimal.Common.Abstractions.Employees;
using ApplicationAnimal.Common.DTO;
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

namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal
{
    public sealed class CreateBoundWithAnimalHandler : ICommandHandler<CreateBoundWithAnimalCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<CreateBoundWithAnimalHandler> _logger;
        private readonly IValidator<CreateBoundWithAnimalCommand> _validator;

        public CreateBoundWithAnimalHandler(IEmployeeRepository employeeRepository, ILogger<CreateBoundWithAnimalHandler> logger, IValidator<CreateBoundWithAnimalCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int, Errors>> Handle(CreateBoundWithAnimalCommand command, CancellationToken cancellationToken)
        {
            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new NotImplementedException();
            }

            // Пересмотреть метод AssignAnimalToEmployee в репозитории чтобы возвращал Result
            await _employeeRepository.AssignAnimalToEmployee(command.CreateBoundWithAnimalDto.employeeId, command.CreateBoundWithAnimalDto.animalId, cancellationToken);

            return -1;
        }
    }
}
