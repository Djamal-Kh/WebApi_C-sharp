using ApplicationAnimal.Services.Animals;
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
    public class DeleteAnimalAsyncTests
    {
        private readonly Mock<ILogger<AnimalService>> _mockLogger;
        private readonly Mock<IAnimalRepository> _mockAnimalRepository;
        public DeleteAnimalAsyncTests()
        {
            _mockLogger = new Mock<ILogger<AnimalService>>();
            _mockAnimalRepository = new Mock<IAnimalRepository>();
        }


        [Fact]
        public async Task DeleteAnimalAsync_ExistingAnimal_ReturnsSuccessMessage()
        {
            // Arrange
            int id = 1;
            var animal = new Lion { Name = "Lion", Id = id };
            string expectedMessage = $"Животное с id = {id} было удалено";

            _mockAnimalRepository
                .Setup(g => g.GetAnimalByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(animal);

            _mockAnimalRepository
                .Setup(d => d.DeleteAnimalAsync(animal, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var service = new AnimalService(
                _mockAnimalRepository.Object,
                _mockLogger.Object);
            // Act
            var result = await service.DeleteAnimalAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be($"Животное с id = {id} было удалено");
        }

        [Fact]
        public async Task DeleteAnimalAsync_NonExistingAnimal_ReturnsKeyNotFoundException()
        {
            // Arrange
            int id = 1;
            int incorrectId = 999;
            var animal = new Lion { Name = "Lion", Id = id };

            _mockAnimalRepository
                .Setup(g => g.GetAnimalByIdAsync(incorrectId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());

            var service = new AnimalService(
                _mockAnimalRepository.Object,
                _mockLogger.Object);

            // Act&Assert
            await service
                .Awaiting(d => d.DeleteAnimalAsync(incorrectId))
                .Should()
                .ThrowAsync<KeyNotFoundException>();           
        }
    }
}

