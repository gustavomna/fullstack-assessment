using Application.Abstractions.Messaging;
using Domain.Departments;
using Domain.Employees;
using SharedKernel;

namespace Application.Departments.GetAll;

internal sealed class GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    : IQueryHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponse>>
{
    public async Task<Result<IEnumerable<DepartmentResponse>>> Handle(
        GetAllDepartmentsQuery query,
        CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAllAsync();

        var response = departments.Select(d => new DepartmentResponse(
            d.Id,
            d.Name,
            d.Description,
            d.CreatedAt,
            d.UpdatedAt,
            d.Employees.Select(e => new EmployeeBasicResponse(
                e.Id,
                e.FirstName,
                e.LastName))
            ));

        return response.ToList();
    }
}
