using DomainAnimal.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Repositories
{
    public class CreateAnimalAsyncTests : RepositoryTestBase
    {
        private readonly AnimalRepository _animalRepository;

        public CreateAnimalAsyncTests()
        {
            _animalRepository = new AnimalRepository(DbContext);
        }


        [Fact]
        public async Task ShouldAddLionToDb_ReturnTask_WhenCreateIsSuccess()
        {
            //Arrange
            var id = 1;
            var animal = new Lion { Name = "TestLion", Id = id};
            var findAnimalBeforeAdding = await DbContext.Animals.FindAsync(id);

            //Act
            await _animalRepository.CreateAnimalAsync(animal);

            //Arrange
            var findAnimalAfterAdding = await DbContext.Animals.FindAsync(id);

            findAnimalBeforeAdding.Should().BeNull();
            findAnimalAfterAdding.Should().NotBeNull();
        }
    }
}
