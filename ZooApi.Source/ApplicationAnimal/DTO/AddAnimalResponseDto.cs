using DomainAnimal.Entities;
using System.Text.Json.Serialization;

namespace WebApiAnimal.DTO
{
    public sealed record AddAnimalResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnumAnimalType Type { get; set; }
        public int? EmployeeId { get; set; }
    }
}
