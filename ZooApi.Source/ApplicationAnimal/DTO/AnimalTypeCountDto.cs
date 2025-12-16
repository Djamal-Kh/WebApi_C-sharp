using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Common.DTO
{
    public sealed record AnimalTypeCountDto
    {
        AnimalType Type { get; set; }
        int Count { get; set; }

        public AnimalTypeCountDto(AnimalType type, int count)
        {
            Type = type;
            Count = count;
        }
    }
}
