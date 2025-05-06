using Application.Abstractions.Messaging;
using Application.Employees.GetAll;

namespace Application.Employees.GetById;

public sealed record GetByIdEmployeeQuery(Guid EmployeeId) : IQuery<EmployeeResponse>;

