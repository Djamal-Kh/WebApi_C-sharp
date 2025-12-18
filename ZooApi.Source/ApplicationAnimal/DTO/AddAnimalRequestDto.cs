using DomainAnimal.Entities;
using System.Text.Json.Serialization;

namespace ZooApi.DTO
{
    public sealed record AddAnimalRequestDto
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
