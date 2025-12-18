using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Shared.Common.ResultParttern;
using Shared.Common.ResultPattern;

namespace DomainAnimal.Entities
{
    public class Employee
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public EnumEmployeePosition Position { get; private set; }
        public int Limit { get; private set; }
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

        public UnitResult<Error> Demotion()
        {
            if (Position == EnumEmployeePosition.Traine)
                return GeneralErrors.ValueIsInvalid();

            Position--;
            Limit = GetLimitForPosition(Position);
            return UnitResult.Success<Error>();
        }

        public UnitResult<Error> Promotion()
        {
            if (Position == EnumEmployeePosition.Senior)
                return GeneralErrors.ValueIsInvalid();

            Position++;
            Limit = GetLimitForPosition(Position);
            return UnitResult.Success<Error>();
        }
    

    public UnitResult<Error> AssignAnimal(Animal animal)
        {
            if (Animals.Count >= Limit)
                return GeneralErrors.ValueIsInvalid($"Превышен лимит ({Limit}) животных для данного сотрудника");

            if (animal.EmployeeId != null)
            {
                if (animal.EmployeeId == this.Id)
                    return GeneralErrors.ValueIsInvalid($"Животное уже закреплено за этим сотрудником");

                return GeneralErrors.ValueIsInvalid($"Животное уже закреплено за другим сотрудником с ID = {animal.EmployeeId} ");
            }

            Animals.Add(animal);
            return UnitResult.Success<Error>();
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
