using Application.Abstractions.Messaging;

namespace Application.Employees.Create;

public sealed record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Phone,
    string Address,
    string Department,
    DateTime HireDate) : ICommand<Guid>;

