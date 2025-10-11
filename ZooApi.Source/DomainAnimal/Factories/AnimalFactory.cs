using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DomainAnimal.Factories
{
    public static class AnimalFactory
    {
        public static Animal Create(AnimalType type, string name, int energy = 50)
        {
            return type switch
            {
                AnimalType.Lion => Lion.Create(name, energy),
                AnimalType.Monkey => Monkey.Create(name, energy),
                _ => throw new ArgumentException()
            };
        }
    }
}
