using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.DemoteEmployee
{
    public class DemoteEmployeeValidator : AbstractValidator<DemoteEmployeeCommand>
    {
        public DemoteEmployeeValidator()
        {
            RuleFor(command => command.employeeId)
                .NotEmpty().WithError(GeneralErrors.ValueIsRequired("employeeId"))
                .GreaterThan(0).WithError(GeneralErrors.ValueIsInvalid("employeeId"));
        }
    }
}
