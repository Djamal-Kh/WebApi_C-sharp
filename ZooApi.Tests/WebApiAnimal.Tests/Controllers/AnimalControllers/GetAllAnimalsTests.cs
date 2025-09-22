﻿using AutoMapper;
using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data.SqlTypes;
using ZooApi.Controllers;
using ZooApi.DTO;

namespace WebApiAnimal.Tests.Controllers.AnimalControllers
{
    public class GetAllAnimalsTests
    {
        private readonly Mock<IAnimalService> _mockAnimalService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CreateAnimalDto>> _mockCreateAnimalDtoValidator;
        private readonly Mock<ILogger<AnimalController>> _mockLogger;

        public GetAllAnimalsTests()
        {
            _mockAnimalService = new Mock<IAnimalService>();
            _mockMapper = new Mock<IMapper>();
            _mockCreateAnimalDtoValidator = new Mock<IValidator<CreateAnimalDto>>();
            _mockLogger = new Mock<ILogger<AnimalController>>();
        }


        [Fact]
        public async Task ShouldGetAnimalsFromDb_ReturnOk_WhenRepositoryNotEmpty()
        {
            //Arrange
            var expectedAnimals = new List<Animal>
            {
                new Lion {Id = 1, Name = "TestLion"},
                new Monkey {Id = 2, Name = "TestMonkey"}
            };

            var expectedDto = new List<AnimalDto>
            {
                new AnimalDto {Id = 1, Name = "TestLion", Type = AnimalType.Lion},
                new AnimalDto {Id = 2, Name = "TestMonkey", Type = AnimalType.Monkey}
            };

            _mockMapper
                .Setup(m => m.Map<List<AnimalDto>>(expectedAnimals))
                .Returns(expectedDto);

            _mockAnimalService
                .Setup(m => m.GetAllAnimalsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAnimals);

            var controller = new AnimalController(
                _mockLogger.Object,
                _mockAnimalService.Object,
                _mockMapper.Object,
                _mockCreateAnimalDtoValidator.Object);

            //Act
            var result = await controller.GetAllAnimals();

            //Assert
            var OkResult = result as OkObjectResult;
            OkResult.Should().NotBeNull();
            OkResult.Should().BeOfType<OkObjectResult>();

            var returnedDto = OkResult.Value as List<AnimalDto>;
            returnedDto.Should().NotBeNull();
            returnedDto.Should().HaveCount(expectedAnimals.Count);

            returnedDto[0].Name.Should().Be(expectedAnimals[0].Name);
            returnedDto[0].Type.Should().Be(expectedAnimals[0].Type);
            returnedDto[0].Id.Should().Be(expectedAnimals[0].Id);
            returnedDto[1].Name.Should().Be(expectedAnimals[1].Name);
            returnedDto[1].Type.Should().Be(expectedAnimals[1].Type);
            returnedDto[1].Id.Should().Be(expectedAnimals[1].Id);
        }


        [Fact]
        public async Task ShouldGetAnimalsFromDb_ReturnSqlNullValueException_WhenRepositoryIsEmpty()
        {
            //Arrange
            _mockAnimalService
                .Setup(m => m.GetAllAnimalsAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SqlNullValueException());

            var controller = new AnimalController(
                _mockLogger.Object,
                _mockAnimalService.Object,
                _mockMapper.Object,
                _mockCreateAnimalDtoValidator.Object);

            //Act & Assert
            await controller
                .Awaiting(c => c.GetAllAnimals())
                .Should()
                .ThrowAsync<SqlNullValueException>();
        }   
    }
}
