using Application.Abstractions.Messaging;
using Domain.Departments;
using SharedKernel;

namespace Application.Departments.Update;

internal sealed class UpdateDepartmentCommandHandler(
    IDepartmentRepository departmentRepository) : ICommandHandler<UpdateDepartmentCommand>
{
    public async Task<Result> Handle(UpdateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(command.Id);

        if (department is null)
        {
            return Result.Failure(DepartmentErrors.NotFound(command.Id));
        }

        department.Name = command.Name;
        department.Description = command.Description;
        department.UpdatedAt = DateTime.UtcNow;

        await departmentRepository.UpdateAsync(department);

        return Result.Success();
    }
}
