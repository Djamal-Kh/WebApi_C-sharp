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

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.PromoteEmployee
{
    public sealed class PromoteEmployeeHandler : ICommandHandler<PromoteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<PromoteEmployeeHandler> _logger;
        private readonly IValidator<PromoteEmployeeCommand> _validator;

        public PromoteEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<PromoteEmployeeHandler> logger, IValidator<PromoteEmployeeCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int, Errors>> Handle(PromoteEmployeeCommand command, CancellationToken cancellationToken)
        {
            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new NotImplementedException();
            }

            // Поменять параметры и ответ у метода в репозитории
            Employee fakeEmployee = new("Name", EnumEmployeePosition.Middle);
            var result = await _employeeRepository.PromotionEmployee(fakeEmployee, cancellationToken);

            return -1;
        }
    }
}
