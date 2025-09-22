using FluentValidation;
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
                .NotEmpty().WithMessage("Поле \"name\" не должно быть пустым");

            RuleFor(x => x.Name)
                .Length(2, 20).WithMessage("Имя должно содержать не менее 2 и не более 20 символов")
                .Must(name => Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я\s]+$")).WithMessage("Не используйте в имени специальные знаки и числа").When(x => !string.IsNullOrWhiteSpace(x.Name)); ;

            RuleFor(x => x.Type)
                .NotNull().WithMessage("Поле \"type\" не должно быть пустым");
        }
    }
}
