using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Employees;
using SharedKernel;

namespace Application.Employees.Delete;

internal sealed class DeleteEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IUserContext userContext) : ICommandHandler<DeleteEmployeeCommand>
{
    public async Task<Result> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(command.EmployeeId);

        if (employee is null)
        {
            return Result.Failure(EmployeeErrors.NotFound(command.EmployeeId));
        }

        await employeeRepository.DeleteAsync(command.EmployeeId);

        return Result.Success();
    }
}

