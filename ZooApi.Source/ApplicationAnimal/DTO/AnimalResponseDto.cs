using DomainAnimal.Entities;

namespace ZooApi.DTO
{
    public sealed record AnimalResponseDto
    {
        public int Id { get; set; }
        public EnumAnimalType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Energy { get; set; } = 50;
        public int? EmployeeId { get; set; } 
    }
}
