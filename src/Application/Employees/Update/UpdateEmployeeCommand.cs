using Application.Abstractions.Messaging;

namespace Application.Employees.Update;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Address,
    string Department) : ICommand;
