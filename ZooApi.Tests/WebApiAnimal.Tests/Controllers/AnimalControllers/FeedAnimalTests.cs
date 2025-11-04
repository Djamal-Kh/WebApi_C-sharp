using ApplicationAnimal.Common.Interfaces;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooApi.Controllers;
using ZooApi.DTO;

namespace WebApiAnimal.Tests.Controllers.AnimalControllers
{
    public class FeedAnimalTests
    {
        private readonly Mock<IAnimalService> _mockAnimalService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CreateAnimalDto>> _mockCreateAnimalDtoValidator;
        private readonly Mock<ILogger<AnimalController>> _mockLogger;

        public FeedAnimalTests() 
        {
            _mockAnimalService = new Mock<IAnimalService>();
            _mockMapper = new Mock<IMapper>();
            _mockCreateAnimalDtoValidator = new Mock<IValidator<CreateAnimalDto>>();
            _mockLogger = new Mock<ILogger<AnimalController>>();
        }


        [Theory]
        [InlineData (50, "Лев увеличивает энергию на 30 единиц")]
        [InlineData (100, "Лев сыт - максимальная энергия была достигнута ранее")]
        public async Task ShouldIncreaseEnergyInDbByIdOfAnimal_ReturnOk_WhenAnimalIsFound(int energy, string serviceResponse)
        {
            //Arrange
            int animalId = 1;

            _mockAnimalService
                .Setup(a => a.FeedAnimalAsync(animalId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(serviceResponse);

            var controller = new AnimalController(
                _mockLogger.Object,
                _mockAnimalService.Object,
                _mockMapper.Object, 
                _mockCreateAnimalDtoValidator.Object);
            
            //Act
            var result = await controller.FeedAnimal(animalId);
             
            //Assert
            var okResult = result as OkObjectResult;
            var response = okResult.Value as string;

            okResult.Should().BeOfType<OkObjectResult>();
            okResult.StatusCode.Should().Be(200);
            response.Should().Be(serviceResponse);
        }


        [Fact]
        public async Task Should_GetAnimals_ReturnKeyNotFoundException_WhenAnimalsIsNotFound()
        {
            //Arrange
            _mockAnimalService
                .Setup(a => a.GetAllAnimalsAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = new AnimalController(
                _mockLogger.Object, 
                _mockAnimalService.Object, 
                _mockMapper.Object, 
                _mockCreateAnimalDtoValidator.Object);

            //Act & Assrt
            await controller
                .Awaiting(c => c.GetAllAnimals())
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }
    }
}
