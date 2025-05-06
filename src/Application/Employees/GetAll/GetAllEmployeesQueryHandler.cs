using Application.Abstractions.Messaging;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Employees.GetAll;

internal sealed class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository)
    : IQueryHandler<GetAllEmployeesQuery, IEnumerable<EmployeeResponse>>
{
    public async Task<Result<IEnumerable<EmployeeResponse>>> Handle(
        GetAllEmployeesQuery query,
        CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.GetAllAsync();

        var response = employees.Select(e => new EmployeeResponse(
            e.Id,
            e.FirstName,
            e.LastName,
            e.HireDate,
            e.Phone,
            e.Address,
            e.DepartmentId,
            e.Department?.Name ?? "N/A"));

        return response.ToList();
    }
}
