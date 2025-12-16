using ApplicationAnimal.Common.DTO;
using Shared.Common.Abstractions.Employees;


namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal
{
    public sealed record CreateBoundWithAnimalCommand(int employeeId, int animalId) : ICommand; 
}
