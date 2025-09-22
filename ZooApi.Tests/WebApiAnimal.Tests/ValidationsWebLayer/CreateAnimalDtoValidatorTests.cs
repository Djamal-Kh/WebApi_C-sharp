using DomainAnimal.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZooApi.DTO;
using ZooApi.Validations;

namespace WebApiAnimal.Tests.ValidationsWebLayer
{
    public class CreateAnimalDtoValidatorTests
    {
        private readonly CreateAnimalDtoValidator _validator;
        public CreateAnimalDtoValidatorTests()
        {
            _validator = new CreateAnimalDtoValidator();
        }

        [Fact]
        public void ValidDto_ShouldPassValidation()
        {
            //Arrange
            var dto = new CreateAnimalDto { Name = "name" };

            //Act
            var result = _validator.Validate(dto);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData (null)]
        [InlineData ("")]
        [InlineData(" ")]
        [InlineData("      ")]
        public void Name_NullOrEmpty_ShouldFailValidation(string? name)
        {
            //Arrange
            var dto = new CreateAnimalDto { Name = name };

            //Act
            var result = _validator.Validate(dto);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Поле \"name\" не должно быть пустым"));
        }

        [Theory]
        [InlineData("s")]
        [InlineData("VeryVeryLongTextForName")]
        public void Name_InvalidLenght_ShouldFailValidation(string name)
        {
            //Arrange
            var dto = new CreateAnimalDto { Name = name };

            //Act
            var result = _validator.Validate(dto);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Имя должно содержать не менее 2 и не более 20 символов");
        }

        [Theory]
        [InlineData("Name#")]
        [InlineData("124A")]
        [InlineData("X*(")]
        [InlineData("F2^l")]
        public void Name_InvalidSymbols_ShouldFailValidation(string name)
        {
            //Arrange
            var dto = new CreateAnimalDto { Name = name };

            //Act
            var result = _validator.Validate(dto);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Не используйте в имени специальные знаки и числа");
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Alex")]
        [InlineData("Gloriya")]
        [InlineData("Melmon")]
        [InlineData("Marti")]
        [InlineData("Monkeys")]
        public void Name_ValidSymbols_ShouldPassValidation(string name)
        {
            //Arrange
            var dto = new CreateAnimalDto { Name = name };

            //Act
            var result = _validator.Validate(dto);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Type_NullOrEmpty_ShouldFailValidation()
        {
            // Не знаю как проверить, т.к. type (AnimalType) не может быть равен null
        }

        [Theory]
        [InlineData(AnimalType.Lion)]
        [InlineData(AnimalType.Monkey)]
        public void Type_ValidEnumValue_ShouldPass(AnimalType type)
        {
            //Arrange
            var dto = new CreateAnimalDto { Name = "Name", Type = type };

            //Act
            var result = _validator.Validate(dto);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData (AnimalType.Monkey, "1")]
        [InlineData(AnimalType.Monkey, "*")]
        [InlineData (AnimalType.Lion, "So!@#$12345LongNameForLion")]
        public void MultipleValidationErrors_ShouldReturnAllErrors(AnimalType type, string name)
        {
            //Arrange
            var dto = new CreateAnimalDto { Type = type, Name = name };

            //Act
            var result = _validator.Validate(dto);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();

            result.Errors.Should().Contain(e => e.ErrorMessage == "Имя должно содержать не менее 2 и не более 20 символов");
            result.Errors.Should().Contain(e => e.ErrorMessage == "Не используйте в имени специальные знаки и числа");
        }

        [Theory]
        [InlineData(AnimalType.Lion, "Lion", true)]
        [InlineData(AnimalType.Lion, "L", false)]
        [InlineData(AnimalType.Lion, "Lion1", false)]
        [InlineData(AnimalType.Lion, "Lion#@", false)]
        public void Validation_ShouldReturnExpectedIsValid(AnimalType type, string name, bool expectedIdValid)
        {
            //Arrange
            var dto = new CreateAnimalDto { Name = name, Type = type };

            //Act
            var result = _validator.Validate(dto);

            //Assert
            result.IsValid.Should().Be(expectedIdValid);
        }
    }
}
