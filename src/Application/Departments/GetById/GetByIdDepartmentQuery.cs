using Application.Abstractions.Messaging;
using Domain.Departments;

namespace Application.Departments.GetById;

public sealed record GetByIdDepartmentQuery(Guid DepartmentId) : IQuery<DepartmentResponse>;
