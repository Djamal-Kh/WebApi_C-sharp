using AutoMapper;
using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApiAnimal.DTO;
using ZooApi.Controllers;
using ZooApi.DTO;

namespace WebApiAnimal.Tests.Controllers.AnimalControllers
{
    public class CreateAnimal
    {
        private readonly Mock<IAnimalService> _mockAnimalService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CreateAnimalDto>> _mockCreateAnimalDtoValidator;
        private readonly Mock<ILogger<AnimalController>> _mockLogger;

        public CreateAnimal()
        {
            _mockAnimalService = new Mock<IAnimalService>();
            _mockMapper = new Mock<IMapper>();
            _mockCreateAnimalDtoValidator = new Mock<IValidator<CreateAnimalDto>>();
            _mockLogger = new Mock<ILogger<AnimalController>>();
        }


        [Theory]
        [InlineData(AnimalType.Lion, "TestLion")]
        [InlineData(AnimalType.Monkey, "TestMonkey")]
        public async Task ShouldAddAnimalsToDb_ReturnOk_WhenCreateSuccess(AnimalType type, string name)
        {
            //Arrange
            var createdAnimal = new Lion(name);
            var createAnimalDto = new CreateAnimalDto { Name = name, Type = type };
            var expectedDto = new AnimalDto { Name = name, Type = type };
            var validationResult = new ValidationResult(); // Success validation

            _mockCreateAnimalDtoValidator
                .Setup(v => v.ValidateAsync(createAnimalDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _mockAnimalService
                .Setup(a => a.CreateAnimalAsync(type, name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdAnimal);

            _mockMapper
                .Setup(m => m.Map<AnimalDto>(createdAnimal))
                .Returns(expectedDto);

            var controller = new AnimalController(
                _mockLogger.Object, 
                _mockAnimalService.Object, 
                _mockMapper.Object, 
                _mockCreateAnimalDtoValidator.Object
                );

            //Act
            var result = await controller.CreateAnimal(createAnimalDto);

            //Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Should().BeOfType<OkObjectResult>();
            okResult.StatusCode.Should().Be(200);

            var animalResult = okResult.Value as AnimalDto;
            animalResult.Should().NotBeNull();
            animalResult.Name.Should().Be(name);
            animalResult.Type.Should().Be(type);
        }


        [Theory]
        [InlineData("Name", "Имя должно содержать не менее 2 и не более 20 символов", "L")]
        [InlineData("Name", "Поле \"name\" не должно быть пустым", "")]
        [InlineData("Name", "Не используйте в имени специальные знаки и числа в имени", "Lion123")]
        [InlineData("Type", "Поле \"type\" не должно быть пустым", "")]
        public async Task ShouldAddAnimalsToDb_ReturnFailValidaton_WhenCreateFailedOnStepValidation(string propertyName, string errorMessage, string inputData)
        {
            //Arrange
            var createDto = new CreateAnimalDto { Type = AnimalType.Lion, Name = inputData };
            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure(propertyName, errorMessage) 
                {
                    AttemptedValue = inputData
                }
            });

            _mockCreateAnimalDtoValidator
                .Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var controller = new AnimalController(
                _mockLogger.Object, 
                _mockAnimalService.Object, 
                _mockMapper.Object, 
                _mockCreateAnimalDtoValidator.Object);

            //Act
            var result = await controller.CreateAnimal(createDto);

            //Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Should().BeOfType<BadRequestObjectResult>();
            badRequest.StatusCode.Should().Be(400);

            var validationErrors = badRequest.Value as IEnumerable<ValidationErrorDto>;
            validationErrors.Should().NotBeNull();
            validationErrors.Count().Should().Be(1);

            var validationError = validationErrors.First();
            validationError.Should().NotBeNull();
            validationError.AttemptedValue.Should().Be(inputData);
            validationError.ErrorMessage.Should().Be(errorMessage);
        }


        [Fact]
        public async Task ShouldAddAnimalsToDb_ReturnArgumentException_WhenNotFoundTypeOfAnimal()
        {
            //Arrange
            var createDto = new CreateAnimalDto { Name = "TestLion", Type = (AnimalType)999 };
            var validationResult = new ValidationResult();

            _mockCreateAnimalDtoValidator
                .Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _mockAnimalService
                .Setup(a => a.CreateAnimalAsync(createDto.Type, createDto.Name, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Argument out of Range"));

            var controller = new AnimalController(
                _mockLogger.Object, 
                _mockAnimalService.Object,
                _mockMapper.Object, 
                _mockCreateAnimalDtoValidator.Object);

            //Act & Assert
            var result = await controller
                .Awaiting(c => c.CreateAnimal(createDto))
                .Should()
                .ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task ShouldAddAnimalsToDb_ReturnValidationException_WhenAnimalWithThatNameAlreadyExist()
        {
            var createDto = new CreateAnimalDto 
            {
                Name = "TestLion", Type = AnimalType.Lion
            };

            var validationResult = new ValidationResult();

            _mockCreateAnimalDtoValidator
                .Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _mockAnimalService
                .Setup(a => a.CreateAnimalAsync(createDto.Type, createDto.Name, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("Already exists with this name"));

            var controller = new AnimalController(
                _mockLogger.Object,
                _mockAnimalService.Object,
                _mockMapper.Object,
                _mockCreateAnimalDtoValidator.Object);

            //Act & Assert
            var result = await controller
                .Awaiting(c => c.CreateAnimal(createDto))
                .Should()
                .ThrowAsync<ValidationException>();
        }
    }
}
