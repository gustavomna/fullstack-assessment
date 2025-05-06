using Application.Abstractions.Messaging;

namespace Application.Departments.Update;

public sealed record UpdateDepartmentCommand(
    Guid Id,
    string Name,
    string Description) : ICommand;
