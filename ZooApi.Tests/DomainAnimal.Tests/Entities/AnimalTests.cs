using DomainAnimal.Entities;
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
        public LionTests()
        {
            _lion = new Lion();
        }

        [Theory]
        [InlineData(30, 60, "ARRRRRRR")]
        [InlineData(80, 100, "ARRRRRRR")]
        [InlineData(100, 100, "Лев наелся")]
        [InlineData(150, 100, "Лев наелся")]
        
        public void Eat_WithVariousEnergy_CorrectBehaviour(int initialEnergy, int expectedEnergy, string expectedResult)
        {
            //Arrange
            _lion.Energy = initialEnergy;

            //Act
            var lionResult = _lion.Eat();

            //Assert
            lionResult.Should().BeEquivalentTo(expectedResult);
            _lion.Energy.Should().Be(expectedEnergy);
        }

        [Fact]
        public void ReturnDefaultValue_WhenLionHasNoName()
        {
            //Arrange
            var lion = new Lion { };
            string expectedString = "NoName";
            int expectedEnergy = 50;

            //Act
            var monkeyResult = lion.Name;

            //Arrange
            lion.Name.Should().Be(expectedString);
            lion.Energy.Should().Be(expectedEnergy);
        }
    }


    public class MonkeyTests
    {
        private readonly Monkey _monkey;
        public MonkeyTests()
        {
            _monkey = new Monkey();
        }

        [Theory]
        [InlineData(30, 80, "UGUGUUUGGUUU")]
        [InlineData(80, 100, "UGUGUUUGGUUU")]
        [InlineData(100, 100, "Обезьяна наелась")]
        [InlineData(150, 100, "Обезьяна наелась")]
        public void Eat_WithVariousEnergy_CorrectBehaviour(int initialEnergy, int expectedEnergy, string expectedResult)
        {
            //Arrange
            _monkey.Energy = initialEnergy;

            //Act
            var monkeyResult = _monkey.Eat();

            //Assert
            monkeyResult.Should().BeEquivalentTo(expectedResult);
            _monkey.Energy.Should().Be(expectedEnergy);
        }

        [Fact]
        public void ReturnDefaultValue_WhenMonkeyHasNoName()
        {
            //Arrange
            var monkey = new Monkey { };
            string expectedString = "NoName";
            int expectedEnergy = 50;

            //Act
            var monkeyResult = monkey.Name;

            //Arrange
            monkey.Name.Should().Be(expectedString);
            monkey.Energy.Should().Be(expectedEnergy);
        }
    }
}
