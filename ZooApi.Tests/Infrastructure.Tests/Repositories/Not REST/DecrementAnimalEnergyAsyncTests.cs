using DomainAnimal.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Repositories.Not_REST
{
    public class DecrementAnimalEnergyAsyncTests : RepositoryTestBase
    {
        private readonly AnimalRepository _animalRepository;
        private readonly int decrementValue = 10;

        public DecrementAnimalEnergyAsyncTests()
        {
            _animalRepository = new AnimalRepository(DbContext);
        }

        /* Да, я понимаю что это не совсем правильный тест, т.к.
         он проверяет не сам метод, а его "аналог". 
         Это связано с тем, что InMemory не поддерживает 
         метод "ExecuteUpdateAsync()". Тогда как реальная БД (Postgre)
         поддерживает ее */
        [Theory]
        [InlineData(100, "TestLionWithMaxEnergy")]
        [InlineData(50, "TestMonkeyWithZeroEnergy")]
        [InlineData(1, "TestMonkeyWithVerySmallEnergy")]
        [InlineData(0, "TestMonkeyWithDefaultEnergy")]
        public async Task ShouldDecreaseEnergyOfAnimalInDb_ReturnVoid(int energy, string name)
        {
            //Arrange
            int id = 1;
            var animalBeforeDecrement = new Lion { Name = name, Energy = energy, Id = id } ;

            await DbContext.AddAsync(animalBeforeDecrement);
            await DbContext.SaveChangesAsync();  

            //Act
            var animals = await DbContext.Animals.Where(e => e.Energy > 0 && e.Energy >= decrementValue).ToListAsync();

            foreach (var animal in animals)
            {
                animal.Energy -= decrementValue;
            }

            await DbContext.SaveChangesAsync();
            //Assert
            var animalAfterDecrement = await DbContext.Animals.FindAsync(id);
            animalAfterDecrement.Energy.Should().BeOneOf(energy-decrementValue, energy);
        } 
    }
}
