using DomainAnimal.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Tests.Repositories
{
    public class FeedAnimalAsyncTests : RepositoryTestBase
    {
        private readonly AnimalRepository _animalRepository;
        public FeedAnimalAsyncTests() 
        {
            _animalRepository = new AnimalRepository(DbContext);
        }

        [Theory]
        [InlineData(50, "ARRRRRRR")]
        [InlineData(110, "Лев наелся")]
        public async Task ReturnStringSuccess_WhenAnimalIsFound(int energy, string expectedString)
        {
            //Arrange
            int id = 1;
            int maxEnergy = 100;
            
            int increaseEnergy = 30; 
            var lion = new Lion { Id = id, Name = "Test", Energy = energy };

            DbContext.Add(lion);
            DbContext.SaveChanges();

            //Act
            string result = await _animalRepository.FeedAnimalAsync(id);

            //Assert
            var animalFromContext = await DbContext.Animals.FindAsync(id);

            result.Should().NotBeNullOrEmpty();
            result.Should().Be(expectedString);

            animalFromContext.Energy.Should().BeOneOf(maxEnergy, energy + increaseEnergy);
        }

        [Fact]
        public async Task ReturnKeyNotFoundException_WhenAnimalsIsNotFound()
        {
            //Arrange
            int incorrectId = 999;
            int id = 1;
            var lion = new Lion { Id = id, Name = "test" };

            DbContext.Add(lion);
            DbContext.SaveChanges();

            await _animalRepository.Awaiting(f => f.FeedAnimalAsync(incorrectId))
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }
    }
}
