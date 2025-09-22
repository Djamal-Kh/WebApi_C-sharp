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
    public class DeleteAnimalAsyncTests : RepositoryTestBase
    {
        private readonly AnimalRepository _animalRepository;
        public DeleteAnimalAsyncTests() 
        {
            _animalRepository = new AnimalRepository(DbContext);
        }

        /* 
         * Других тестов (сценариев) нет т.к. сюда ГАРАНТИРОВАННО попадают
         * животные и никогда не попает null при нормальном\запланированным 
         * использованием приложения
         */ 
        [Fact]
        public async Task CheckRemoveFromDb_ReturnTask_WhenAnimalIsExisting()
        {
            //Arrange
            int id = 1;
            var animal = new Lion {Id = id};
            DbContext.Add(animal);
            DbContext.SaveChanges();

            var animalBeforeDelete = await DbContext.Animals.FindAsync(id);

            //Act
            await _animalRepository.DeleteAnimalAsync(animal);

            //Assert
            var animalAfterDelete = await DbContext.Animals.FindAsync(id);

            animalBeforeDelete.Should().NotBeNull();
            animalAfterDelete.Should().BeNull();
        }
    }
}
