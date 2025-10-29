using DomainAnimal.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DomainAnimal.Entities
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
        public int Id { get; protected set; }
        public AnimalType Type { get; protected set; }
        public string Name { get; protected set; }
        public int Energy { get; protected set; }
        public Guid SomeSecretInformation { get; private set; }
        public int? EmployeeId { get; protected set; }
        public Employee Employee { get; protected set; }

        // конструктор без параметров для EF Core
        protected Animal() { }
        protected Animal(AnimalType type, string name, int energy)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException();

            if(energy < 0 || energy > 100)
                throw new ArgumentOutOfRangeException();

            Type = type;
            Name = name;
            Energy = energy;
        }

        public abstract string Eat();
        public abstract string MakeSound();
    }

    public class Lion : Animal
    {
        private const int LionEnegryGain = 30;

        private Lion(string name, int energy) : base(AnimalType.Lion, name, energy) { }

        public static Lion Create(string name, int energy)
        {
            return new Lion(name, energy);
        }
        public override string MakeSound()
        {
            return "ARRRRRRR";
        }
        public override string Eat()
        {
            if (Energy >= MaxEnergy)
            {
                Energy = MaxEnergy;
                return "Лев наелся";
            }

            else
            {
                Energy += LionEnegryGain;

                if (Energy >= MaxEnergy)
                    Energy = MaxEnergy;

                return MakeSound();
            }
        }
    }

    public class Monkey : Animal
    {
        private const int MonkeyEnegryGain = 50;

        private Monkey(string name, int energy) : base(AnimalType.Monkey, name, energy) { }

        public static Monkey Create(string name, int energy)
        {
            return new Monkey(name, energy);
        }

        public override string MakeSound()
        {
            return "UGUGUUUGGUUU";
        }

        public override string Eat()
        {
            if (Energy >= MaxEnergy)
            {
                Energy = MaxEnergy;
                return "Обезьяна наелась";
            }
            else
            {
                Energy += MonkeyEnegryGain;

                if (Energy >= MaxEnergy)
                    Energy = MaxEnergy;

                return MakeSound();
            }
        }
    }
}
