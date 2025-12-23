using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Abstractions.Employees;
using Shared.Common.ResultPattern;
using Shared.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationAnimal.Services.Caching;
using Microsoft.Extensions.Caching.Hybrid;
using DomainAnimal;

namespace ApplicationAnimal.Services.Employees.Command.DeleteCommands.DeleteEmployee
{
    public sealed class DeleteEmployeeHandler : ICommandHandler<DeleteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DeleteEmployeeHandler> _logger;
        private readonly IValidator<DeleteEmployeeCommand> _validator;
        private readonly HybridCache _cache;

        public DeleteEmployeeHandler(IEmployeeRepository employeeRepository, 
            ILogger<DeleteEmployeeHandler> logger, 
            IValidator<DeleteEmployeeCommand> validator,
            HybridCache cache)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
            _cache = cache;
        }

        public async Task<UnitResult<Errors>> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteEmployeeCommand for EmployeeId: {EmployeeId}",
                command.employeeId);

            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for DeleteEmployeeCommand: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return validationResult.ToList();
            }

            // Необходимо узать поле position для инвалидации кэша (Жесткий костыль)
            var employeeInfo = await _employeeRepository.GetByIdAsync(command.employeeId, cancellationToken);

            // Удаление сотрудника
            var result = await _employeeRepository.FireEmployeeAsync(command.employeeId, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to delete employee with ID {EmployeeId}: {Error}",
                    command.employeeId, result.Error);

                return result.Error.ToErrors();
            }
                
            _logger.LogInformation("Successfully deleted employee with ID {EmployeeId}",
                command.employeeId);

            // Инвалидация кэша
            var tags = new List<string> {
                EmployeeConstants.EMPLOYEE_CACHE_TAG,
                EmployeeConstants.EMPLOYEE_BY_ID_CACHE_TAG + command.employeeId,
                AnimalConstants.ANIMAL_CACHE_TAG,
                AnimalConstants.ALL_ANIMALS_BY_ID_CACHE_TAG
            };

            await _cache.RemoveByTagAsync(tags, cancellationToken);

            // Передать вызывающему методу успешный результат
            return UnitResult.Success<Errors>();
        }
    }
}
