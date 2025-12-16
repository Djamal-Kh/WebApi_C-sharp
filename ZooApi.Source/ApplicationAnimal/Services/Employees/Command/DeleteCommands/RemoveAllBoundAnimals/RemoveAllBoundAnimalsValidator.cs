using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.DeleteCommands.RemoveAllBoundAnimals
{
    public class RemoveAllBoundAnimalsValidator : AbstractValidator<RemoveAllBoundAnimalsCommand>
    {
        public RemoveAllBoundAnimalsValidator()
        {
            RuleFor(command => command.employeeId)
                .NotEmpty().WithError(GeneralErrors.ValueIsRequired("employeeId"))
                .GreaterThan(0).WithError(GeneralErrors.ValueIsInvalid("employeeId"));
        }
    }
}
