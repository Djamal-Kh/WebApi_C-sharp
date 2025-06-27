using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryAnimals
{
    public abstract class Animal
    {
        private static int _nextId = 1;
        public Animal(string name)
        {
            Name = name;
            Id = _nextId++;
        }
        public string Name { get; set; }
        protected int Energy { get; set; }
        public int Id { get; set; } 
    }

    public class Lion : Animal, IAnimal
    {
        public Lion(string name) : base(name) {}

        public string MakeSound()
        {
            return "ARRRRRRR ";
        }
        public string Eat()
        {
            if (Energy >= 100)
            {
                return "The lion is full, stop feeding him!";
            }
            else
            {
                string Sound = MakeSound();
                Energy += 50;
                return Sound + "You fed the lion ";
            }
        }
    }

    public class Monkey : Animal, IAnimal
    {
        public Monkey(string name) : base(name) {}
        public string MakeSound()
        {
            return "UGUGUUUGGUUU";
        }


        public string Eat()
        {
            if (Energy >= 100)
            {
                return "The monkey is full, stop feeding him!";
            }
            else
            {               
                string Sound = MakeSound();
                Energy += 33;
                return Sound + "You fed the monkey ";
            }
        }
    }
}
