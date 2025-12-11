using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateEmployee
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeValidator() 
        {
            RuleFor(command => command.CreateEmployeeRequest.Name)
              .NotNull().WithError(GeneralErrors.ValueIsRequired("Поле \"name\" не должно быть пустым"))
              .NotEmpty().WithError(GeneralErrors.ValueIsRequired("Поле \"name\" не должно быть пустым"));

            RuleFor(command => command.CreateEmployeeRequest.Name)
                .MaximumLength(20).WithError(GeneralErrors.ValueIsInvalid("Имя должно содержать не более 20 символов"))
                .Must(name => Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я\s]+$"))
                    .WithError(GeneralErrors.ValueIsInvalid("Не используйте в имени специальные знаки и числа"))
                    .When(command => !string.IsNullOrWhiteSpace(command.CreateEmployeeRequest.Name)); ;

            RuleFor(command => command.CreateEmployeeRequest.Position)
                .NotNull().WithError(GeneralErrors.ValueIsRequired("Поле \"Position\" не должно быть пустым"));
        }
    }
}
