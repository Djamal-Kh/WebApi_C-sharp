using ApplicationAnimal.Common.Abstractions.Employees;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Services.Employees.Command.Create
{
    public sealed class CreateEmployeeHandler : ICommandHandler<CreateEmployeeCommand>
    {
        public Task<Result> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
