namespace Shared.Common.Abstractions.Employees
{
    public interface ICommand : IBaseCommand;

    public interface ICommand<TResponse> : IBaseCommand;

    public interface IBaseCommand;
}
