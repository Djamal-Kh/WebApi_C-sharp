using DomainAnimal.Entities;
using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;
using System.Text.RegularExpressions;
using ZooApi.DTO;

namespace ZooApi.Validations
{
    public class AddAnimalDtoValidator : AbstractValidator<AddAnimalRequestDto>
    {
        public AddAnimalDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithError(GeneralErrors.ValueIsRequired("Поле \"name\" не должно быть пустым"))
                .NotEmpty().WithError(GeneralErrors.ValueIsRequired("Поле \"name\" не должно быть пустым"));

            RuleFor(x => x.Name)
                .Length(2, 20).WithError(GeneralErrors.ValueIsInvalid("Имя должно содержать не менее 2 и не более 20 символов"))
                .Must(name => Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я\s]+$"))
                    .WithError(GeneralErrors.ValueIsInvalid("Не используйте в имени специальные знаки и числа"))
                    .When(x => !string.IsNullOrWhiteSpace(x.Name)); ;

            RuleFor(x => x.Type)
                .NotNull().WithError(GeneralErrors.ValueIsRequired("Поле \"Type\" не должно быть пустым"))
                .Must(isValidValueAnimalEnum).WithError(GeneralErrors.ValueIsInvalid("Некорректное значение для Type"));
        }

        private bool isValidValueAnimalEnum(string typeString)
        {
            if (int.TryParse(typeString, out int enumValue))
                return Enum.IsDefined(typeof(EnumAnimalType), enumValue);

            if (Enum.TryParse(typeof(EnumAnimalType), typeString, ignoreCase: true, out _))
                return true;

            return false;
        }
    }
}
