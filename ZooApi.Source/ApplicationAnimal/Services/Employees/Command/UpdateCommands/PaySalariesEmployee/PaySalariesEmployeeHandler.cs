using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Microsoft.Extensions.Logging;
using Shared.Common.Abstractions.Employees;
using Shared.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ICommand = Shared.Common.Abstractions.Employees.ICommand;

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.PaySalariesEmployee
{
    public class PaySalariesEmployeeHandler
    {
        private readonly ILogger<PaySalariesEmployeeHandler> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public PaySalariesEmployeeHandler(ILogger<PaySalariesEmployeeHandler> logger,
            IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public async Task<UnitResult<Errors>> Handle(CancellationToken cancellationToken)
        { 
            try
            {
                _logger.LogInformation("Starting to pay salaries to employees.");

                foreach (EnumEmployeePosition employeePosition in Enum.GetValues(typeof(EnumEmployeePosition)))
                {
                    int salary = Employee.CalculateSalary(employeePosition);

                    await _employeeRepository.PaySalariesAsync(employeePosition, salary, cancellationToken);
                }

                return UnitResult.Success<Errors>();
            }
            catch
            {
                _logger.LogError("Error occurred while paying salaries to employees.");

                return GeneralErrors.Failure().ToErrors();
            }
        }
    }
}
