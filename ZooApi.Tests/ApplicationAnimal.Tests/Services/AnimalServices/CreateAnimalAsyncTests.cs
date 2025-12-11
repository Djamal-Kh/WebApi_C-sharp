using ApplicationAnimal.Services.Animals;
using Castle.Core.Logging;
using DomainAnimal.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationAnimal.Tests.Services.AnimalServices
{
    public class CreateAnimalAsyncTests
    {
        private readonly Mock<ILogger<AnimalService>> _mockLogger;
        private readonly Mock<IAnimalRepository> _mockAnimalRepository;

        public CreateAnimalAsyncTests()
        {
            _mockLogger = new Mock<ILogger<AnimalService>>();
            _mockAnimalRepository = new Mock<IAnimalRepository>();
        }

        [Fact]
        public async Task ShouldAddLionToDb_ReturnLion_WhenLionCreateSuccess()
        {
            //Arrange
            string name = "Lion";
            var lion = new Lion { Name = name, Id = 1 };

            _mockAnimalRepository
                .Setup(en => en.ExistsByName(name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockAnimalRepository
                .Setup(c => c.CreateAnimalAsync(lion, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
                

            var service = new AnimalService(
                _mockAnimalRepository.Object,
                _mockLogger.Object);

            //Act
            var result = await service.CreateAnimalAsync(AnimalType.Lion, name);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(name);
            result.Should().BeOfType<Lion>();
        }

        [Fact]
        public async Task ShouldAddAnimalsToDb_ReturnArgumentException_WhenNotFoundTypeOfAnimal()
        {
            //Arrange
            var type = (AnimalType)999;
            var animal = new Lion { Name = "Animal", Type = type };

            _mockAnimalRepository
                .Setup(c => c.CreateAnimalAsync(animal, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException());

            var service = new AnimalService(
                _mockAnimalRepository.Object,
                _mockLogger.Object);

            //Act & Assert 
            await service.Awaiting(c => c.CreateAnimalAsync(type, animal.Name))
                .Should()
                .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task ShouldAddAnimalsToDb_ReturnValidationException_WhenAnimalWithThatNameAlreadyExist()
        {
            //Arrange
            _mockAnimalRepository
                .Setup(en => en.ExistsByName("Name", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = new AnimalService(
                _mockAnimalRepository.Object,
                _mockLogger.Object);

            //Act & Assert
            await service.Awaiting(c => c.CreateAnimalAsync(AnimalType.Lion, "Name"))
                .Should()
                .ThrowAsync<ValidationException>();
        }
    }
}

