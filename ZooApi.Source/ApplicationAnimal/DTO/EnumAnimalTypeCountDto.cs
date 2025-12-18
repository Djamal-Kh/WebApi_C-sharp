using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Common.DTO
{
    public sealed record EnumAnimalTypeCountDto
    {
        public EnumAnimalType Type { get; set; }
        public int Count { get; set; }

        public EnumAnimalTypeCountDto(EnumAnimalType type, int count)
        {
            Type = type;
            Count = count;
        }
    }
}
