using Application.Departments.Create;
using Application.Departments.Delete;
using Application.Departments.GetAll;
using Application.Departments.GetById;
using Application.Departments.Update;
using Domain.Departments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.Api.Controllers.Departments;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly ISender _sender;

    public DepartmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllDepartmentsQuery();
        Result<IEnumerable<DepartmentResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : this.Problem(result.Error);
    }

    [HttpGet("{departmentId:guid}")]
    public async Task<IActionResult> GetById(Guid departmentId, CancellationToken cancellationToken)
    {
        var query = new GetByIdDepartmentQuery(departmentId);
        Result<DepartmentResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : this.Problem(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(
            request.Name,
            request.Description);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { departmentId = result.Value }, result.Value)
            : this.Problem(result.Error);
    }

    [HttpPut("{departmentId:guid}")]
    public async Task<IActionResult> Update(Guid departmentId, [FromBody] UpdateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateDepartmentCommand(
            departmentId,
            request.Name,
            request.Description);

        Result result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : this.Problem(result.Error);
    }

    [HttpDelete("{departmentId:guid}")]
    public async Task<IActionResult> Delete(Guid departmentId, CancellationToken cancellationToken)
    {
        var command = new DeleteDepartmentCommand(departmentId);
        Result result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : this.Problem(result.Error);
    }
}