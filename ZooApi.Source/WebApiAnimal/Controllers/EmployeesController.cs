
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateEmployee;
using ApplicationAnimal.Services.Employees.Command.DeleteCommands.DeleteEmployee;
using ApplicationAnimal.Services.Employees.Command.DeleteCommands.RemoveAllBoundAnimals;
using ApplicationAnimal.Services.Employees.Command.UpdateCommands.DemoteEmployee;
using ApplicationAnimal.Services.Employees.Command.UpdateCommands.PromoteEmployee;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Abstractions.Employees;

namespace WebApiAnimal.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public sealed class EmployeesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(
            [FromServices] ICommandHandler<int, CreateEmployeeCommand> handler,
            [FromBody] CreateEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateEmployeeCommand(request);

            var result = await handler.Handle(command, cancellationToken);

            return Ok(result);
        }

        [HttpPost("employees/{employeeId}/animals/{animalId}")]
        public async Task<IActionResult> CreateBoundWithAnimal(
            [FromServices] ICommandHandler<string, CreateBoundWithAnimalCommand> handler,
            [FromRoute] int employeeId,
            [FromRoute] int animalId,
            CancellationToken cancellationToken)
        {
            var command = new CreateBoundWithAnimalCommand(employeeId, animalId);

            var result = await handler.Handle(command, cancellationToken);

            return Ok(result);
        }

        [HttpPatch("employees/{employeeId}/promote")]
        public async Task<IActionResult> PromoteEmployee(
            [FromServices] ICommandHandler<int, PromoteEmployeeCommand> handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var command = new PromoteEmployeeCommand(employeeId);

            var result = await handler.Handle(command, cancellationToken);

            return Ok(result);
        }

        [HttpPatch("employees/{employeeId}/demote")]
        public async Task<IActionResult> DemoteEmployee(
            [FromServices] ICommandHandler<int, DemoteEmployeeCommand> handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var command = new DemoteEmployeeCommand(employeeId);

            var result = await handler.Handle(command, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("employees/{employeeId}/deleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(
            [FromServices] ICommandHandler<DeleteEmployeeCommand> handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteEmployeeCommand(employeeId);

            var result = await handler.Handle(command, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("employees/{employeeId}/removeBounds")]
        public async Task<IActionResult> RemoveAllBoundAnimals(
            [FromServices] ICommandHandler<RemoveAllBoundAnimalsCommand> handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var command = new RemoveAllBoundAnimalsCommand(employeeId);

            var result = await handler.Handle(command, cancellationToken);

            return Ok(result);
        }
    }
}
