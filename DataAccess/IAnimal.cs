using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace DataAccess
{

    [JsonDerivedType(typeof(Lion), typeDiscriminator: "Lion")]
    [JsonDerivedType(typeof(Monkey), typeDiscriminator: "Monkey")]
    public interface IAnimal
    {
        public string MakeSound();
        public string Eat();
    }
}
