namespace Domain.Employees;

public sealed record EmployeeBasicResponse(
    Guid Id,
    string FirstName,
    string LastName);