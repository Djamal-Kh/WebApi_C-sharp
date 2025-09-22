using DomainAnimal.Entities;
using System.Text.Json.Serialization;

namespace ZooApi.DTO
{
    public class CreateAnimalDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AnimalType Type { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
