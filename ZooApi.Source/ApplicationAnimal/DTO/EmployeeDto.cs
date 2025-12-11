using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Common.DTO
{
    public sealed record EmployeeDto()
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public EnumEmployeePosition Position { get; set; }
        public int Limit { get; set; }
    }
}
