using Application.Abstractions.Messaging;
using Domain.Departments;
using Domain.Employees;
using SharedKernel;

namespace Application.Departments.GetById;

internal sealed class GetByIdDepartmentQueryHandler(IDepartmentRepository departmentRepository)
    : IQueryHandler<GetByIdDepartmentQuery, DepartmentResponse>
{
    public async Task<Result<DepartmentResponse>> Handle(
        GetByIdDepartmentQuery query,
        CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(query.DepartmentId);

        if (department is null)
        {
            return Result.Failure<DepartmentResponse>(DepartmentErrors.NotFound(query.DepartmentId));
        }

        var response = new DepartmentResponse(
            department.Id,
            department.Name,
            department.Description,
            department.CreatedAt,
            department.UpdatedAt,
            department.Employees.Select(e => new EmployeeBasicResponse(
                e.Id,
                e.FirstName,
                e.LastName))
            );

        return response;
    }
}
