using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAnimal.Entities
{
    public class Employee
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public EnumEmployeePosition Position { get; private set; }
        private int Limit { get; set; }
        public ICollection<Animal> Animals { get; private set; }

        // конструктор без параметров для EF Core
        protected Employee() { }
        public Employee(string name, EnumEmployeePosition position)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException();

            Name = name;
            Position = position;
            Limit = GetLimitForPosition(position);
        }


        private int GetLimitForPosition(EnumEmployeePosition position)
        {
            return position switch
            {
                EnumEmployeePosition.Traine => 1,
                EnumEmployeePosition.Junior => 3,
                EnumEmployeePosition.Middle => 5,
                EnumEmployeePosition.Senior => 10,

                _ => throw new ArgumentOutOfRangeException()

            };
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
            if (Position == EnumEmployeePosition.Senior)
                return "Нельзя повысить, и так максимальная должность !";

            Position--;
            return $"Повышен до {Position}";
        }
    }


    public enum EnumEmployeePosition
    {
        Traine = 1,
        Junior,
        Middle,
        Senior,
    }
}
