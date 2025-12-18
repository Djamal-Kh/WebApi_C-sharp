using DomainAnimal.Entities;
using System.Text.Json.Serialization;

namespace ApplicationAnimal.DTO
{
    public sealed record GetNumberAnimalsTypeResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnumAnimalType Type { get; set; }
        public int Count { get; set; }
    }
}
