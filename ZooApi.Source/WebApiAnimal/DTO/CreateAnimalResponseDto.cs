using DomainAnimal.Entities;
using System.Text.Json.Serialization;

namespace WebApiAnimal.DTO
{
    public sealed record CreateAnimalResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AnimalType Type { get; set; }
    }
}
