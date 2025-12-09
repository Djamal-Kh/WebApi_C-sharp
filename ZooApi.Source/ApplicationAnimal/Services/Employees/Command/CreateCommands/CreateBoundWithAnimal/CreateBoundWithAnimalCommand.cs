using ApplicationAnimal.Common.DTO;
using ICommand = ApplicationAnimal.Common.Abstractions.Employees.ICommand;

namespace ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal
{
    public sealed record CreateBoundWithAnimalCommand(int employeeId, int animalId) : ICommand; 
}
