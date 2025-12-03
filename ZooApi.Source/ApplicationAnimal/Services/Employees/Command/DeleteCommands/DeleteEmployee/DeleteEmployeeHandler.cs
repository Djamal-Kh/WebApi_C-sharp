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

namespace ApplicationAnimal.Services.Employees.Command.DeleteCommands.DeleteEmployee
{
    public sealed class DeleteEmployeeHandler : ICommandHandler<DeleteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DeleteEmployeeHandler> _logger;
        private readonly IValidator<DeleteEmployeeCommand> _validator;

        public DeleteEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<DeleteEmployeeHandler> logger, IValidator<DeleteEmployeeCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<UnitResult<Errors>> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new NotImplementedException();
            }

            // Удаление сущности из БД - пересмотреть метод в репозитории чтобы возвращал Result
            var result = await _employeeRepository.FireEmployeeAsync(command.employeeId, cancellationToken);

            if (result.IsFailure)
                return GeneralErrors.ValueIsInvalid().ToErrors();

            return UnitResult.Success<Errors>();
        }
    }
}
