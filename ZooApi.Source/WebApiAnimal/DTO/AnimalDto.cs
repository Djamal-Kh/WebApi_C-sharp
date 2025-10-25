using DomainAnimal.Entities;

namespace ZooApi.DTO
{
    public sealed class AnimalDto
    {
        public int Id { get; set; }
        public AnimalType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Energy { get; set; } = 50;
    }
}
