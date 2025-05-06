using Application.Abstractions.Messaging;

namespace Application.Employees.Delete;

public sealed record DeleteEmployeeCommand(Guid EmployeeId) : ICommand;

