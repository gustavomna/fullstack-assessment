using Application.Abstractions.Messaging;
using Application.Employees.GetAll;
using Domain.Employees;
using SharedKernel;

namespace Application.Employees.GetById;

internal sealed class GetByIdEmployeeQueryHandler(IEmployeeRepository employeeRepository)
    : IQueryHandler<GetByIdEmployeeQuery, EmployeeResponse>
{
    public async Task<Result<EmployeeResponse>> Handle(
        GetByIdEmployeeQuery query,
        CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(query.EmployeeId);

        if (employee is null)
        {
            return Result.Failure<EmployeeResponse>(EmployeeErrors.NotFound(query.EmployeeId));
        }

        var response = new EmployeeResponse(
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.HireDate,
            employee.Phone,
            employee.Address,
            employee.DepartmentId,
            employee.Department?.Name ?? "N/A");

        return response;
    }
}

