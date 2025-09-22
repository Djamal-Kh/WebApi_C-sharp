using AutoMapper;
using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooApi.Controllers;
using ZooApi.DTO;

namespace WebApiAnimal.Tests.Controllers.AnimalControllers
{
    public class DeleteAnimalTests
    {
        private readonly Mock<IAnimalService> _mockAnimalService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CreateAnimalDto>> _mockCreateAnimalDtoValidator;
        private readonly Mock<ILogger<AnimalController>> _mockLogger;
        public DeleteAnimalTests() 
        {
            _mockAnimalService = new Mock<IAnimalService>();
            _mockMapper = new Mock<IMapper>();
            _mockCreateAnimalDtoValidator = new Mock<IValidator<CreateAnimalDto>>();
            _mockLogger = new Mock<ILogger<AnimalController>>();
        }


        [Fact]
        public async Task ShouldDeleteAnimalById_ReturnOk_WhenAnimalIsFound()
        {
            //Arrange
            var lion = new Lion("TestLion");
            int animalId = 1;
            string serviceResponse = $"Животное с id = {animalId} было удалено";

            _mockAnimalService
                .Setup(a => a.DeleteAnimalAsync(animalId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(serviceResponse);

            var controller = new AnimalController(
                _mockLogger.Object,
                _mockAnimalService.Object,
                _mockMapper.Object,
                _mockCreateAnimalDtoValidator.Object);

            //Act
            var result = await controller.DeleteAnimal(animalId);

            //Assert
            var okResult = result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as string;
            response.Should().Be(serviceResponse);
        }


        [Fact]
        public async Task ShouldDeleteAnimalById_ReturnNotFound_WhenAnimalNotFound()
        {
            //Arrange
            _mockAnimalService
                .Setup(d => d.DeleteAnimalAsync(999, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = new AnimalController(
                _mockLogger.Object,
                _mockAnimalService.Object,
                _mockMapper.Object, 
                _mockCreateAnimalDtoValidator.Object);

            //Act & Assert
            await controller.
                Awaiting(d => d.DeleteAnimal(999))
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }
    }
}
