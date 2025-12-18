using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateEmployee;
using ApplicationAnimal.Services.Employees.Command.DeleteCommands.DeleteEmployee;
using ApplicationAnimal.Services.Employees.Command.DeleteCommands.RemoveAllBoundAnimals;
using ApplicationAnimal.Services.Employees.Command.UpdateCommands.DemoteEmployee;
using ApplicationAnimal.Services.Employees.Command.UpdateCommands.PromoteEmployee;
using ApplicationAnimal.Services.Employees.Queries;
using DomainAnimal.Entities;
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

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            int createdEmployeeId = result.Value;

            return Ok(createdEmployeeId);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(
            [FromServices] GetEmployeesHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(cancellationToken);

            return Ok(result);
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(
            [FromServices] GetEmployeeByIdHandler handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(employeeId, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("employees-without-animals")]
        public async Task<IActionResult> GetEmployeesWithoutAnimals(
            [FromServices] GetEmployeesWithoutAnimalsHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet("by-positions/{position}")]
        public async Task<IActionResult> GetEmployeesByPositions(
            [FromServices] GetEmployeesByPositionsHandler handler,
            [FromRoute] EnumEmployeePosition position,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(position, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("employees/{employeeId}/animals/{animalId}")]
        public async Task<IActionResult> CreateBoundWithAnimal(
            [FromServices] ICommandHandler<string, CreateBoundWithAnimalCommand> handler,
            [FromRoute] int employeeId,
            [FromRoute] int animalId,
            CancellationToken cancellationToken)
        {
            var command = new CreateBoundWithAnimalCommand(employeeId, animalId);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            string boundInfo = result.Value;

            return Ok(boundInfo);
        }

        [HttpPatch("employees/{employeeId}/promote")]
        public async Task<IActionResult> PromoteEmployee(
            [FromServices] ICommandHandler<EnumEmployeePosition, PromoteEmployeeCommand> handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var command = new PromoteEmployeeCommand(employeeId);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error); 
            }

            EnumEmployeePosition newPositionLevel = result.Value;

            return Ok(newPositionLevel);
        }

        [HttpPatch("employees/{employeeId}/demote")]
        public async Task<IActionResult> DemoteEmployee(
            [FromServices] ICommandHandler<EnumEmployeePosition, DemoteEmployeeCommand> handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var command = new DemoteEmployeeCommand(employeeId);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            EnumEmployeePosition newPositionLevel = result.Value;

            return Ok(newPositionLevel);
        }

        [HttpDelete("employees/{employeeId}/deleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(
            [FromServices] ICommandHandler<DeleteEmployeeCommand> handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteEmployeeCommand(employeeId);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error); 
            }

            return Ok(employeeId);
        }

        [HttpDelete("employees/{employeeId}/removeBounds")]
        public async Task<IActionResult> RemoveAllBoundAnimals(
            [FromServices] ICommandHandler<RemoveAllBoundAnimalsCommand> handler,
            [FromRoute] int employeeId,
            CancellationToken cancellationToken)
        {
            var command = new RemoveAllBoundAnimalsCommand(employeeId);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(employeeId);
        }
    }
}
