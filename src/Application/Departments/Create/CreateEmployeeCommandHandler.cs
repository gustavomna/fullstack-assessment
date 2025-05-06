using Application.Abstractions.Messaging;
using Domain.Departments;
using SharedKernel;

namespace Application.Departments.Create;

internal sealed class CreateDepartmentCommandHandler(
    IDepartmentRepository departmentRepository) : ICommandHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow
        };

        department.Raise(new DepartmentCreatedDomainEvent(department.Id));

        await departmentRepository.AddAsync(department);

        return department.Id;
    }
}
