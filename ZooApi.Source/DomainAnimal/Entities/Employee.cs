using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAnimal.Entities
{
    public enum EnumEmployeePosition
    {
        Junior = 1,
        Senior,
        Managers
    }
    public class Employee
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public EnumEmployeePosition Position { get; private set; }
        public ICollection<Animal> Animals { get; private set; }
    }
}
