using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    public enum AnimalType
    {
        Lion,
        Monkey,
    }

    public abstract class Animal : IAnimal
    {
        [Key]
        public int Id { get; set; }

        public AnimalType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Energy { get; set; } = 50;

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
        public Lion() : base() { }
        public Lion(string name) : base(AnimalType.Lion,name) {}

        public override string MakeSound()
        {
            return "ARRRRRRR ";
        }
        public override string Eat()
        {
            if (Energy >= 100)
            {
                return "Лев наелся";
            }
            else
            {
                Energy += 50;
                return MakeSound();
            }
        }
    }

    public class Monkey : Animal
    {
        public Monkey() : base() { }
        public Monkey(string name) : base(AnimalType.Monkey,name) {}
        public override string MakeSound()
        {
            return "UGUGUUUGGUUU";
        }

        public override string Eat()
        {
            if (Energy >= 100)
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
