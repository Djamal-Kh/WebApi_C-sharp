using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace Shared.Common.Abstractions.Employees
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
