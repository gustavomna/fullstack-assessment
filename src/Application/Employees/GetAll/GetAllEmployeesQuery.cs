using Application.Abstractions.Messaging;

namespace Application.Employees.GetAll;

public sealed record GetAllEmployeesQuery() : IQuery<IEnumerable<EmployeeResponse>>;
