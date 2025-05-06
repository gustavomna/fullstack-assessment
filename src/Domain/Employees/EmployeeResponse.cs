namespace Application.Employees.GetAll;

public sealed record EmployeeResponse(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime HireDate,
    string Phone,
    string Address,
    Guid DepartmentId,
    string DepartmentName);
