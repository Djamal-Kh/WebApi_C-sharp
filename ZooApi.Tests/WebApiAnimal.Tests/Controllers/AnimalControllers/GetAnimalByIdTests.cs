using AutoMapper;
using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ZooApi.Controllers;
using ZooApi.DTO;

namespace WebApiAnimal.Tests.Controllers.AnimalControllers
{
    public class GetAnimalByIdTests
    {
        private readonly Mock<IAnimalService> _mockAnimalService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CreateAnimalDto>> _mockCreateAnimalDtoValidator;
        private readonly Mock<ILogger<AnimalController>> _mockLogger;
        
        public GetAnimalByIdTests()
        {
            _mockAnimalService = new Mock<IAnimalService>();
            _mockMapper = new Mock<IMapper>();
            _mockCreateAnimalDtoValidator = new Mock<IValidator<CreateAnimalDto>>();
            _mockLogger = new Mock<ILogger<AnimalController>>();
        }


        [Fact]
        public async Task ShouldGetAnimalFromDbById_ReturnOk_WhenAnimalIsFound()
        {
            //Arrange
            var lion = new Lion("TestLion");
            var expectedDto = new AnimalDto { Id = lion.Id, Name = "TestLion" };

            _mockMapper
                .Setup(m => m.Map<AnimalDto>(lion))
                .Returns(expectedDto);

            _mockAnimalService
                .Setup(a => a.GetAnimalByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(lion);

            var controller = new AnimalController(
                _mockLogger.Object,
                _mockAnimalService.Object,
                _mockMapper.Object,
                _mockCreateAnimalDtoValidator.Object);

            //Act
            var result = await controller.GetAnimalById(1);

            //Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Should().BeOfType<OkObjectResult>();
            okResult.StatusCode.Should().Be(200);

            var returnedDto = okResult.Value as AnimalDto;
            returnedDto.Should().NotBeNull();
            returnedDto.Id.Should().Be(lion.Id);
            returnedDto.Name.Should().Be(lion.Name);
        }

        [Fact]
        public async Task ShouldGetAnimalFromDbById_ReturnKeyNotFoundException_WhenAnimalIsNotFound()
        {
            //Arrange
            _mockAnimalService
                .Setup(a => a.GetAnimalByIdAsync(999, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = new AnimalController(
                _mockLogger.Object, 
                _mockAnimalService.Object, 
                _mockMapper.Object,
                _mockCreateAnimalDtoValidator.Object);

            //Act & Assert
            await controller
                .Awaiting(c => c.GetAnimalById(999))
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }
    }
}
