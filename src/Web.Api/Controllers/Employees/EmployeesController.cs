using Application.Employees.Create;
using Application.Employees.Delete;
using Application.Employees.GetAll;
using Application.Employees.GetById;
using Application.Employees.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.Api.Controllers.Employees;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllEmployeesQuery();
        Result<IEnumerable<EmployeeResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : this.Problem(result.Error);
    }

    [HttpGet("{employeeId:guid}")]
    public async Task<IActionResult> GetById(Guid employeeId, CancellationToken cancellationToken)
    {
        var query = new GetByIdEmployeeQuery(employeeId);
        Result<EmployeeResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : this.Problem(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateEmployeeCommand(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Address,
            request.Department,
            request.HireDate);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { employeeId = result.Value }, result.Value)
            : this.Problem(result.Error);
    }

    [HttpPut("{employeeId:guid}")]
    public async Task<IActionResult> Update(Guid employeeId, [FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateEmployeeCommand(
            employeeId,
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Address,
            request.Department);

        Result result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : this.Problem(result.Error);
    }

    [HttpDelete("{employeeId:guid}")]
    public async Task<IActionResult> Delete(Guid employeeId, CancellationToken cancellationToken)
    {
        var command = new DeleteEmployeeCommand(employeeId);
        Result result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : this.Problem(result.Error);
    }
}