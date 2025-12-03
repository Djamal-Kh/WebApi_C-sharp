using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Common.Abstractions.Employees
{
    public interface ICommandHandler<TResponse, in TCommand> 
        where TCommand : ICommand
    {
        Task<Result<TResponse, Errors>> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
    {
        Task<UnitResult<Errors>> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
