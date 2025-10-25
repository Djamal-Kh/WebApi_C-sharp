using DomainAnimal.Entities;
using System.Text.Json.Serialization;

namespace DomainAnimal.Interfaces
{

    [JsonDerivedType(typeof(Lion), typeDiscriminator: "Lion")]
    [JsonDerivedType(typeof(Monkey), typeDiscriminator: "Monkey")]
    public interface IAnimal
    {
        public string MakeSound();
        public string Eat();
    }
}
