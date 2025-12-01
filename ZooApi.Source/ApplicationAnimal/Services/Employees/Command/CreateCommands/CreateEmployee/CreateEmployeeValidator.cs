using FluentValidation;
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
              .NotNull().WithMessage("Поле \"name\" не должно быть пустым")
              .NotEmpty().WithMessage("Поле \"name\" не должно быть пустым");

            RuleFor(command => command.CreateEmployeeRequest.Name)
                .Length(20).WithMessage("Имя должно содержать не более 20 символов")
                .Must(name => Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я\s]+$"))
                    .WithMessage("Не используйте в имени специальные знаки и числа")
                    .When(command => !string.IsNullOrWhiteSpace(command.CreateEmployeeRequest.Name)); ;

            RuleFor(command => command.CreateEmployeeRequest.Position)
                .NotNull().WithMessage("Поле \"Position\" не должно быть пустым");
        }
    }
}
