using SharedKernel;

namespace Domain.Departments;

public sealed record DepartmentCreatedDomainEvent(Guid DepartmentId) : IDomainEvent;
