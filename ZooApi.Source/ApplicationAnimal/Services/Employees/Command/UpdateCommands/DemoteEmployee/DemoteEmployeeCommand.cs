using ApplicationAnimal.Common.Abstractions.Employees;
using ApplicationAnimal.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.UpdateCommands.DemoteEmployee
{
    public sealed record DemoteEmployeeCommand(int employeeId) : ICommand;
}
