using ApplicationAnimal.Common.Abstractions.Animals;
using ApplicationAnimal.Services.Animals;
using Castle.Core.Logging;
using DomainAnimal.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Tests.Services.AnimalServices
{
    public class GetAnimalByIdAsyncTests
    {
        /* На самом деле покрывать тестами этот сервис
           не имеет смысла (по крайней мере на данном этапе)
           т.к. его роль - просто вызывать метод из репозитория. 
           Следовательно, на уровне контроллера уже протестированы
           всевозможные сценарии. В Application слое нет доп. логики*/ 

        private readonly Mock<ILogger<AnimalService>> _mockLogger;
        private readonly Mock<IAnimalRepository> _mockRepository;
        public GetAnimalByIdAsyncTests() 
        { 
            _mockLogger = new Mock<ILogger<AnimalService>>();
            _mockRepository = new Mock<IAnimalRepository>();
        }

        [Fact]
        public async Task GetAnimalById_ExistingAnimal_ReturnsAnimal()
        {
            //Arrange
            int id = 1;
            var animal = new Lion { Id = id, Name = "Test" };
            
            _mockRepository
                .Setup(g => g.GetAnimalByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(animal);

            var service = new AnimalService(
                _mockRepository.Object,
                _mockLogger.Object);

            //Act
            var result = await service.GetAnimalByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test");
            result.Id.Should().Be(id);
            result.Should().BeOfType<Lion>();
        }

        [Fact]
        public async Task GetAnimalById_NonExistingAnimal_ReturnsKeyNotFoundException()
        {
            //Arrange
            int id = 1;
            int incorrectId = 999;
            var animal = new Lion { Id = id, Name = "Test" };

            _mockRepository
                .Setup(g => g.GetAnimalByIdAsync(incorrectId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());

            var service = new AnimalService(
                _mockRepository.Object,
                _mockLogger.Object);

            //Act & Assert
            await service
                .Awaiting(g => g.GetAnimalByIdAsync(incorrectId))
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }
    }
}
