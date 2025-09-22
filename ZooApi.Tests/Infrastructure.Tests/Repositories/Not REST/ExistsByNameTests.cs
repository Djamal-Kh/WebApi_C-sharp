using DomainAnimal.Entities;
using FluentAssertions;
using FluentAssertions.Primitives;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Repositories.Not_REST
{
    public class ExistsByNameTests : RepositoryTestBase
    {
        private readonly AnimalRepository _animalRepository;
        public ExistsByNameTests()
        { 
            _animalRepository = new AnimalRepository(DbContext);
        }

        [Fact]
        public async Task ShouldCheckExistAnimalByName_ReturnTrue_WhenAnimalExist()
        {
            //Arrange
            var animal = new Lion("TestLion");
            string name = animal.Name;
            
            DbContext.Add(animal);
            DbContext.SaveChanges();

            //Act
            var result = await _animalRepository.ExistsByName(name);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldCheckExistAnimalByName_ReturnFalse_WhenAnimalNotExist()
        {
            //Arrange
            var animal = new Lion("TestLion");
            string nonExistentName = "Lion";

            DbContext.Add(animal);
            DbContext.SaveChanges();

            //Act
            var result = await _animalRepository.ExistsByName(nonExistentName);

            //Assert
            result.Should().BeFalse();
        }
    }
}
