using Application.Abstractions.Messaging;
using Domain.Departments;
using SharedKernel;

namespace Application.Departments.Delete;

internal sealed class DeleteDepartmentCommandHandler(
    IDepartmentRepository departmentRepository) : ICommandHandler<DeleteDepartmentCommand>
{
    public async Task<Result> Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(command.DepartmentId);

        if (department is null)
        {
            return Result.Failure(DepartmentErrors.NotFound(command.DepartmentId));
        }

        await departmentRepository.DeleteAsync(command.DepartmentId);

        return Result.Success();
    }
}
