using ApplicationAnimal.Common.Abstractions.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.DeleteCommands.RemoveAllBoundAnimals
{
    public sealed record RemoveAllBoundAnimalsCommand(int employeeId) : ICommand;
}
