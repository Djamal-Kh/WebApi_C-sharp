using FluentValidation;
using FluentValidation.Validators;
using FluentValidation.AspNetCore;

using ZooApi.DTO;
using System.Text.RegularExpressions;

namespace ZooApi.Validations
{
    public class CreateAnimalDtoValidator : AbstractValidator<CreateAnimalDto>
    {
        public CreateAnimalDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Поле \"name\" не должно быть пустым")
                .Length(2, 20).WithMessage("Имя должно содержать не менее 2 и не более 20 символов")
                .Must(name => Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я\s]+$")).WithMessage("Не используйте в имени специальные знаки");

            RuleFor(x => x.Type)
                .NotNull().WithMessage("Поле \"type\" не должно быть пустым");
        }
    }
}
