
using Domain.Employees;

namespace Domain.Departments;

public sealed record DepartmentResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<EmployeeBasicResponse> Employees);