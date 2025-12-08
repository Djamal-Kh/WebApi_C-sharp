using DomainAnimal.Entities;

namespace DomainAnimal.Factories
{
    public static class AnimalFactory
    {
        public static Animal Create(EnumAnimalType type, string name, int energy = 50)
        {
            return type switch
            {
                EnumAnimalType.Lion => Lion.Create(name, energy),
                EnumAnimalType.Monkey => Monkey.Create(name, energy),
                _ => throw new ArgumentException()
            };
        }
    }
}
