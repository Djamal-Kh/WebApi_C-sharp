using DomainAnimal.Entities;

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
