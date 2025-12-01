using ApplicationAnimal.Common.Abstractions.Employees;
using ApplicationAnimal.Common.ResultPattern;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.DemoteEmployee
{
    public sealed class DemoteEmployeeHandler : ICommandHandler<DemoteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DemoteEmployeeHandler> _logger;
        private readonly IValidator<DemoteEmployeeCommand> _validator;

        public DemoteEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<DemoteEmployeeHandler> logger, IValidator<DemoteEmployeeCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int, Errors>> Handle(DemoteEmployeeCommand command, CancellationToken cancellationToken)
        {
            // валидация
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new NotImplementedException();
            }

            // Поменять параметры и ответ у метода в репозитории
            Employee fakeEmployee = new("Name", EnumEmployeePosition.Middle);
            var result = await _employeeRepository.DemotionEmployee(fakeEmployee, cancellationToken);

            return -1;
        }
    }
}
