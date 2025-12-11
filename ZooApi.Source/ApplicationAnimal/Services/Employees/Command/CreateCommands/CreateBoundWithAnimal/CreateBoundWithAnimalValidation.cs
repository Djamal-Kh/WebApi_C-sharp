using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal
{
    public class CreateBoundWithAnimalValidation : AbstractValidator<CreateBoundWithAnimalCommand>
    {
        public CreateBoundWithAnimalValidation()
        {
            RuleFor(command => command.employeeId)
                .NotEmpty().WithError(GeneralErrors.ValueIsRequired("employeeId"))
                .GreaterThan(0).WithError(GeneralErrors.ValueIsInvalid("employeeId"));  
            
            RuleFor(command => command.animalId)
                .NotEmpty().WithError(GeneralErrors.ValueIsRequired("animalId"))
                .GreaterThan(0).WithError(GeneralErrors.ValueIsInvalid("animalId"));
        }
    }
}
