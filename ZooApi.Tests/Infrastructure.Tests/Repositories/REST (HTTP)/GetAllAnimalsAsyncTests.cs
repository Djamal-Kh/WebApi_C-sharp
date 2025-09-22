using DomainAnimal.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Repositories
{
    public class GetAllAnimalsAsyncTests : RepositoryTestBase
    {
        private readonly AnimalRepository _animalRepository;

        public GetAllAnimalsAsyncTests()
        {
            _animalRepository = new AnimalRepository(DbContext);
        }

        [Fact]
        public async Task ReturnOk_WhenContextNotEmpty()
        {
            //Arrange
            var lion = new Lion { Id = 1, Name = "Lion" };
            var monkey = new Monkey { Id = 2, Name = "Monkey" };
            int countOfAnimals = 2;

            DbContext.Add(lion);
            DbContext.Add(monkey);
            DbContext.SaveChanges();

            //Act
            var result = await _animalRepository.GetAllAnimalsAsync();

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain(lion);
            result.Should().Contain(monkey);
            result.Count.Should().Be(countOfAnimals);
        }

        [Fact]
        public async Task ReturnSqlNullValueException_WhenRepositoryIsEmpty()
        {       
            //Act & Assert
            var result = await _animalRepository.Awaiting(g => g.GetAllAnimalsAsync())
                .Should()
                .ThrowAsync<SqlNullValueException>();
        }
    }
}
