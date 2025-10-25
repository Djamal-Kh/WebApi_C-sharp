using DomainAnimal.Entities;
using DomainAnimal.Factories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAnimal.Tests.Entities
{
    public class LionTests
    {
        private readonly Lion _lion;

        [Theory]
        [InlineData(30, 60, "ARRRRRRR")]
        [InlineData(80, 100, "ARRRRRRR")]
        [InlineData(100, 100, "Лев наелся")]
        [InlineData(150, 100, "Лев наелся")]
        
        public void Eat_WithVariousEnergy_CorrectBehaviour(int initialEnergy, int expectedEnergy, string expectedResult)
        {
            //Arrange
            Lion lion = Lion.Create("Name", energy: initialEnergy);

            //Act
            var lionResult = _lion.Eat();

            //Assert
            lionResult.Should().BeEquivalentTo(expectedResult);
            _lion.Energy.Should().Be(expectedEnergy);
        }
    }


    public class MonkeyTests
    {
        private readonly Monkey _monkey;

        [Theory]
        [InlineData(30, 80, "UGUGUUUGGUUU")]
        [InlineData(80, 100, "UGUGUUUGGUUU")]
        [InlineData(100, 100, "Обезьяна наелась")]
        [InlineData(150, 100, "Обезьяна наелась")]
        public void Eat_WithVariousEnergy_CorrectBehaviour(int initialEnergy, int expectedEnergy, string expectedResult)
        {
            //Arrange
            Monkey monkey = Monkey.Create("Name", energy: initialEnergy);

            //Act
            var monkeyResult = _monkey.Eat();

            //Assert
            monkeyResult.Should().BeEquivalentTo(expectedResult);
            _monkey.Energy.Should().Be(expectedEnergy);
        }
    }
}
