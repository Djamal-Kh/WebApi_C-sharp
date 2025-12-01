using ApplicationAnimal.Common.Abstractions.Employees;
using ApplicationAnimal.Common.DTO;
using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateEmployee
{
    public sealed record CreateEmployeeCommand(CreateEmployeeRequest CreateEmployeeRequest) : ICommand;
}
