using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using FluentAssertions;
using Infrastructure.ContextsDb;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Repositories
{
    public class GetAnimalByIdAsyncTests : RepositoryTestBase
    {
        private readonly AnimalRepository _animalRepository;

        public GetAnimalByIdAsyncTests()
        {
            // переменная DbContext взята из абстрактного класса (RepositoryTestBase)
            _animalRepository = new AnimalRepository(DbContext);
        }


        [Fact]
        public async Task ReturnData_WhenAnimalWithSpecifedIdExist()
        {
            //Arrange
            int id = 1;
            var lion = new Lion { Id = id, Name = "Test" };

            DbContext.Add(lion);
            DbContext.SaveChanges();

            //Act
            var result = await _animalRepository.GetAnimalByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test");
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task ReturnKeyNotFoundException_WhenAnimalIsNotFound()
        {
            //Arrange
            var id = 1;
            var incorrectId = 999;
            var lion = new Lion { Id = id, Name = "Test" };

            DbContext.Add(lion);
            DbContext.SaveChanges();

            //Act & Assert
            await _animalRepository
                .Awaiting(g => g.GetAnimalByIdAsync(incorrectId))
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }
    }
}
