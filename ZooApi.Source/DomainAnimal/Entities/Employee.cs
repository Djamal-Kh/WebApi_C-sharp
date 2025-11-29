using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAnimal.Entities
{
    public enum EnumEmployeePosition
    {
        Junior,
        Senior,
        Managers
    }
    public class Employee
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public EnumEmployeePosition Position { get; private set; }
        public ICollection<Animal> Animals { get; private set; }

        // конструктор без параметров для EF Core
        protected Employee() { }
        public Employee(EnumEmployeePosition position, string name)
        {
            if(string.IsNullOrEmpty(Name))
                throw new ArgumentNullException();

            Name = name;
            Position = position;
        }
        public string Demotion()
        {
            if (Position == EnumEmployeePosition.Junior)
                return "Нельзя понизить, только уволить !";

            Position--;
            return $"Понижен до {Position}";
        }

        public string Promotion()
        {
            if (Position == EnumEmployeePosition.Managers)
                return "Нельзя повысить, и так максимальная должность !";

            Position--;
            return $"Повышен до {Position}";
        }
    }
}
