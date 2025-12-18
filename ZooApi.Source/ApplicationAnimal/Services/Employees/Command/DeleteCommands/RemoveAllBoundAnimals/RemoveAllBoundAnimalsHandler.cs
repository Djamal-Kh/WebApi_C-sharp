using Shared.Common.Abstractions.Employees;
using Shared.Common.ResultPattern;
using Shared.Common.Extensions;
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

        public async Task<UnitResult<Errors>> Handle(RemoveAllBoundAnimalsCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RemoveAllBoundAnimalsCommand for EmployeeId: {EmployeeId}",
                command.employeeId);

            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for RemoveAllBoundAnimalsCommand: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return validationResult.ToList();
            }

            // удаление всех связанных животных с сотрудником
            var result = await _employeeRepository.RemoveAllBoundAnimalsAsync(command.employeeId, cancellationToken);
            
            if (result.IsFailure)
            {
                _logger.LogError("Failed to remove all bound animals for EmployeeId: {EmployeeId}. Errors: {Errors}",
                    command.employeeId, string.Join(", ", result.Error));

                return GeneralErrors.ValueIsInvalid().ToErrors();
            }
                
            _logger.LogInformation("Successfully removed all bound animals for EmployeeId: {EmployeeId}",
                command.employeeId);

            return UnitResult.Success<Errors>();
        }
    }
}
