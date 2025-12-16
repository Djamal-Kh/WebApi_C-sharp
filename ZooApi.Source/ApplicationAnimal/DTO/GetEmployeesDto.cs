using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Common.DTO
{
    public sealed record GetEmployeesDto(List<EmployeeDto> Employees);
}
