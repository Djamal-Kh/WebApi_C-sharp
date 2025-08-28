using DomainAnimal.Interfaces;
using System.ComponentModel.DataAnnotations;


namespace DataAccess
{
    public enum AnimalType
    {
        Lion,
        Monkey,
    }

    public abstract class Animal : IAnimal
    {
        protected const int MaxEnergy = 100;
        [Key]
        public int Id { get; set; }

        public AnimalType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Energy { get; set; } = 50;

        public Guid SecretInformation { get; set; }

        public Animal(AnimalType type, string name)
        {
            Type = type;
            Name = name;
        }
        protected Animal() { }

        public abstract string Eat();
        public abstract string MakeSound();

    }


    public class Lion : Animal
    {
        private const int LionEnegryGain = 50;
        public Lion() : base() { }
        public Lion(string name) : base(AnimalType.Lion, name) { }

        public override string MakeSound()
        {
            return "ARRRRRRR ";
        }
        public override string Eat()
        {
            if (Energy >= MaxEnergy)
            {
                return "Лев наелся";
            }
            else
            {
                Energy += LionEnegryGain;
                return MakeSound();
            }
        }
    }

    public class Monkey : Animal
    {
        public Monkey() : base() { }
        public Monkey(string name) : base(AnimalType.Monkey, name) { }
        public override string MakeSound()
        {
            return "UGUGUUUGGUUU";
        }

        public override string Eat()
        {
            if (Energy >= MaxEnergy)
            {
                return "Обезьяна наелась";
            }
            else
            {
                Energy += 50;
                return MakeSound();
            }
        }
    }
}
