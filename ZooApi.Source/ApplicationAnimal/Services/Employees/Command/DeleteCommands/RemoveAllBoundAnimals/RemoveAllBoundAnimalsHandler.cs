using ApplicationAnimal.Common.Abstractions.Employees;
using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.DeleteCommands.RemoveAllBoundAnimals
{
    public sealed class RemoveAllBoundAnimalsHandler : ICommandHandler<RemoveAllBoundAnimalsCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<RemoveAllBoundAnimalsHandler> _logger;
        private readonly IValidator<RemoveAllBoundAnimalsCommand> _validator;

        public RemoveAllBoundAnimalsHandler(IEmployeeRepository employeeRepository, ILogger<RemoveAllBoundAnimalsHandler> logger, IValidator<RemoveAllBoundAnimalsCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int, Errors>> Handle(RemoveAllBoundAnimalsCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new NotImplementedException();
            }

            // Пересмотреть метод RemoveAllBoundAnimals в репозитории чтобы возвращал Result
            await _employeeRepository.RemoveAllBoundAnimals(command.employeeId, cancellationToken);
            return -1;
        }
    }
}
