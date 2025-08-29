using DomainAnimal.Entities;

namespace ZooApi.DTO
{
    public class CreateAnimalDto
    {
        public AnimalType Type { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
