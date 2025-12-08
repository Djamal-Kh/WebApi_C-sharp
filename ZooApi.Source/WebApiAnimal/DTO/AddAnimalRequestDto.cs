using DomainAnimal.Entities;
using System.Text.Json.Serialization;

namespace ZooApi.DTO
{
    public sealed record AddAnimalRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnumAnimalType Type { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
