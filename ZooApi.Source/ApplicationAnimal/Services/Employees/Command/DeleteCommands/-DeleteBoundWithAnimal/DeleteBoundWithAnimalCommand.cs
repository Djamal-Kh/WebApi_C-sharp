using ApplicationAnimal.Common.Abstractions.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.DeleteCommands.DeleteBoundWithAnimal
{
    //  Перекинь метод в Animal
    public sealed record DeleteBoundWithAnimalCommand(int animalId) : ICommand; 
}
